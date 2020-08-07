using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
using Services.SMS.OTP;
using WebFramework.Application.Handlers.Auth;
using WebFramework.Application.Interfaces.Otp;
using WebFramework.Application.Models;

namespace WebFramework.Application.Handlers.Otp
{
    public class OtpHandler : IOtpHandler, IScopedDependency
    {
        private readonly IOtpService _otpService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<Entities.User.User> _userManager;
        private readonly SiteSettings _siteSetting;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly ITokenFactory _tokenFactory;

        public OtpHandler(IOtpService otpService, IUserRepository userRepository, UserManager<Entities.User.User> userManager,
            IOptionsSnapshot<SiteSettings> settings, ILogger<LoginHandler> logger, IJwtService jwtService, ITokenFactory tokenFactory)
        {
            _otpService = otpService;
            _userRepository = userRepository;
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _tokenFactory = tokenFactory;
            _siteSetting = settings.Value;
        }

        public async Task<TokenSelectRequest> Handle(OtpValidationRequest input, CancellationToken cancellationToken)
        {
            var isCodeValid = _otpService.ValidateCode(input.Code, input.Token, input.Phone);
            if (!isCodeValid)
            {
                throw new AppException(ApiResultStatusCode.ServerError, "خطایی رخ داده است.");
            }
            var user = await _userRepository.TableNoTracking
                .SingleOrDefaultAsync(u => u.PhoneNumber.Equals(input.Phone), cancellationToken);

            if (user == null)
            {
                var isUserCreated = await SignUpUser(input.Phone);
                if (!isUserCreated)
                {
                    throw new AppException(ApiResultStatusCode.ServerError, "ثبت نام کاربر با مشکل مواجه شد!");
                }
            }

            var result = await SignInUser(input.Phone, cancellationToken);
            return result;
        }

        private async Task<bool> SignUpUser(string phone)
        {
            var user = new Entities.User.User
            {
                UserName = phone,
                PhoneNumber = phone
            };
            var userCreationResult = await _userManager.CreateAsync(user);
            return userCreationResult.Succeeded;
        }

        private async Task<TokenSelectRequest> SignInUser(string phone, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Table.Include(u => u.RefreshToken)
                .SingleOrDefaultAsync(u => u.PhoneNumber.Equals(phone), cancellationToken);

            if (user == null)
                throw new BadRequestException("کاربری با این شماره تلفن وجود ندارد!", HttpStatusCode.BadRequest);

            var refreshToken = _tokenFactory.GenerateToken();
            HandleRefreshToken(user, refreshToken);
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

        private void HandleRefreshToken(Entities.User.User user, string refreshToken)
        {
            var rt = new RefreshToken(refreshToken, DateTime.UtcNow.AddDays(_siteSetting.JwtSettings.RefreshTokenExpirationDays), user.Id);
            if (user.RefreshToken == null)
            {
                user.RefreshToken = rt;
            }
            else
            {
                user.RefreshToken.Token = rt.Token;
                user.RefreshToken.Expires = rt.Expires;
            }
        }
    }
}
