using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using Data.Contracts;
using Entities.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebFramework.Api;

namespace MyApi.Controllers.v2.Auth
{
    [ApiVersion("2")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/auth-google")]
    public class GoogleAuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public GoogleAuthController(SignInManager<User> signInManager, UserManager<User> userManager,
            IUserRepository userRepository, IConfiguration configuration)
        {
            _signInManager = signInManager;   
            _userManager = userManager;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        
        [HttpPost("validate-token")]
        public async Task<GoogleJsonWebSignature.Payload> ValidateToken([FromQuery] string idToken, CancellationToken cancellationToken)
        {
            //var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            //Assert.NotNull(validPayload, "GoogleValidationResponse", "Id_Token is not valid!");

            //return Ok("Token is Valid");

            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string>() { _configuration.GetSection("Authentication:Google")["ClientId"] };
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;

        }

        //[HttpPost("validate-token")]
        //public async Task<GoogleJsonWebSignature.Payload> ValidateToken([FromQuery] string code, CancellationToken cancellationToken)
        //{
        //    IConfigurationSection googleAuthSection = _configuration.GetSection("Authentication:Google");

        //    var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        //    {
        //        ClientSecrets = new ClientSecrets
        //        {
        //            ClientId = googleAuthSection["ClientId"],
        //            ClientSecret = googleAuthSection["ClientSecret"]
        //        }
        //    });

        //    var redirectUrl = "http://localhost:4200";
        //    var response = await flow.ExchangeCodeForTokenAsync(string.Empty, code, redirectUrl, cancellationToken);

        //    GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
        //    {
        //        Audience = new List<string>() {googleAuthSection["ClientId"]}
        //    };

        //    var payload = await GoogleJsonWebSignature.ValidateAsync(response.IdToken, settings);
        //    return payload;
        //}
    }
}
