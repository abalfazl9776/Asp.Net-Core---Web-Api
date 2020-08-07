using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.JWT;
using WebFramework.Application.Interfaces.Auth;
using WebFramework.Application.Models;

namespace WebFramework.Application.Handlers.Auth
{
    public class RefreshTokenHandler : IRefreshTokenHandler, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly SiteSettings _siteSetting;
        private readonly ITokenFactory _tokenFactory;

        public RefreshTokenHandler(IUserRepository userRepository, ILogger<LoginHandler> logger, IJwtService jwtService,
            ITokenFactory tokenFactory, IOptionsSnapshot<SiteSettings> settings)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _siteSetting = settings.Value;
            _tokenFactory = tokenFactory;
        }

        public async Task<TokenSelectRequest> Handle(RefreshTokenDto rtd, CancellationToken cancellationToken)
        {
            var cp = _jwtService.GetPrincipalFromToken(rtd.access_token, _siteSetting.JwtSettings.SecretKey);

            // invalid token/signing key was passed and we can't extract user claims
            if (cp == null)
            {
                throw new BadRequestException("Unable to extract claims out of the provided token!", HttpStatusCode.BadRequest);
            }

            var id = cp.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var user = await _userRepository.TableNoTracking.Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.Id.Equals(int.Parse(id.Value)), cancellationToken);

            var refreshTokenIsValid = user.RefreshToken != null &&
                                      (user.RefreshToken.Active && user.RefreshToken.Token.Equals(rtd.refresh_token));
            if (!refreshTokenIsValid)
            {
                throw new BadRequestException("Refresh-Token is not valid!", HttpStatusCode.BadRequest);
            }

            var refreshToken = _tokenFactory.GenerateToken();
            var rt = new RefreshToken(refreshToken, DateTime.UtcNow.AddDays(_siteSetting.JwtSettings.RefreshTokenExpirationDays), user.Id);
            user.RefreshToken.Token = rt.Token;
            user.RefreshToken.Expires = rt.Expires;
            await _userRepository.UpdateSecurityStampAsync(user, cancellationToken);

            var jwt = await _jwtService.GenerateAsync(user);
            var token = new TokenSelectRequest
            {
                access_token = jwt.access_token,
                expires_in = jwt.expires_in,
                refresh_token = refreshToken
            };

            return token;
        }
    }
}
