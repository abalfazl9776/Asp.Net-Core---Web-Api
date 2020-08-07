using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Exceptions;
using Data.Contracts;
using Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Services.JWT;
using WebFramework.Api;
using WebFramework.Application.Interfaces.Auth;
using WebFramework.Application.Models;

namespace MyApi.Controllers.v1.Auth
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly ILogger<UserController.UserController> _logger;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly ILoginHandler _loginHandler;
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        private readonly ILogoutHandler _logoutHandler;

        public AuthController(ILogger<UserController.UserController> logger, IJwtService jwtService,
            UserManager<User> userManager, ILoginHandler loginHandler, IRefreshTokenHandler refreshTokenHandler,
            ILogoutHandler logoutHandler)
        {
            _logger = logger;
            _jwtService = jwtService;
            _userManager = userManager;
            _loginHandler = loginHandler;
            _refreshTokenHandler = refreshTokenHandler;
            _logoutHandler = logoutHandler;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Token([FromForm]TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            //if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
            //    throw new Exception("OAuth flow is not password.");

            var user = await _userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new BadRequestException("Username or password is wrong!");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("Username or password is wrong!");

            var jwt = await _jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost("[action]")]
        public async Task<ApiResult<TokenSelectRequest>> Login(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Login called with username: " + tokenRequest.username);
            //if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
            //    throw new Exception("OAuth flow is not password.");
            
            //var user = await _userManager.FindByNameAsync(tokenRequest.username);
            var response = await _loginHandler.Handle(tokenRequest, cancellationToken);
            return response;

        }

        [HttpPost("[action]")]
        public async Task<ApiResult<TokenSelectRequest>> RefreshToken(RefreshTokenDto rtd, CancellationToken cancellationToken)
        {
            var response = await _refreshTokenHandler.Handle(rtd, cancellationToken);
            return response;
        }

        [HttpPost("[action]")]
        public async Task<ApiResult<string>> Logout(CancellationToken cancellationToken)
        {
            var response = await _logoutHandler.Handle(HttpContext, cancellationToken);
            if (response.Equals(ApiResultStatusCode.Success))
            {
                return Ok("Logged out!");
            }
            if (response.Equals(ApiResultStatusCode.NotFound))
            {
                return NotFound("There is no Refresh Token");
            }

            throw new AppException(ApiResultStatusCode.ServerError);
        }
    }
}
