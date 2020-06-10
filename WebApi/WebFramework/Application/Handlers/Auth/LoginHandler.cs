using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Services.JWT;
using WebFramework.Application.Interfaces.Auth;
using WebFramework.Application.Models;

namespace WebFramework.Application.Handlers.Auth
{
    public class LoginHandler : ILoginHandler, IScopedDependency
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<Entities.User.User> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public LoginHandler(IUserRepository userRepository, ILogger<LoginHandler> logger, IJwtService jwtService,
            UserManager<Entities.User.User> userManager, ITokenFactory tokenFactory)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _tokenFactory = tokenFactory;
        }

        public async Task<TokenSelectRequest> Handle(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            var user = await _userRepository.TableNoTracking.Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.UserName.Equals(tokenRequest.username), cancellationToken);

            if (user == null)
                throw new BadRequestException("Username or password is wrong!", HttpStatusCode.BadRequest);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("Username or password is wrong!", HttpStatusCode.BadRequest);

            var refreshToken = _tokenFactory.GenerateToken();
            user.AddRefreshToken(refreshToken, user.Id);
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
