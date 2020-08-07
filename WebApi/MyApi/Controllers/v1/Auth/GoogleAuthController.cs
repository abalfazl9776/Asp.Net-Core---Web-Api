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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebFramework.Api;

namespace MyApi.Controllers.v1.Auth
{
    [ApiVersion("1")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/auth-google")]
    public class GoogleAuthController : BaseController
    {
        protected readonly SignInManager<User> _signInManager;
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


        [HttpGet("[action]")]
        //[Route("api/v1/auth-google/SignInWithGoogle")]
        public IActionResult SignInWithGoogle(string returnUrl, string provider = "Google")
        {
            // Request a redirect to the external login provider.
            //string redirectUrl = Url.Action(nameof(HandleExternalLogin).ToString(), "GoogleAuth", new { returnUrl });
            string redirectUrl = $"api/v1/auth-google/HandleExternalLogin?returnUrl={returnUrl}";
            AuthenticationProperties properties = _signInManager.
                ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        
        [HttpGet("[action]")]
        //[Route("api/v1/auth-google/HandleExternalLogin")]
        public async Task<IActionResult> HandleExternalLogin([FromQuery]string returnUrl = null)
        {
            //string identityExternalCookie = Request.Cookies["Identity.External"];//do we have the cookie??

            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) return new RedirectResult($"{returnUrl}?error=externalsigninerror");

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                if (returnUrl == null)
                {
                    returnUrl = "http://localhost:4200";
                }
                //CredentialsDTO credentials = _authService.ExternalSignIn(info);
                return new RedirectResult($"{returnUrl}?token={info.Principal.FindFirstValue(ClaimTypes.Email)}");
            }

            if (result.IsLockedOut)
            {
                return new RedirectResult($"{returnUrl}?error=lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.

                string loginprovider = info.LoginProvider;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                string name = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                string surname = info.Principal.FindFirstValue(ClaimTypes.Surname);

                return new RedirectResult($"{returnUrl}?error=notregistered&provider={loginprovider}" +
                                          $"&email={email}&name={name}&surname={surname}");
            }
        }

        [HttpGet("[action]")]
        //[Route("/ap1/v1/auth-google/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("http://localhost:4200");
        }

        [HttpGet("[action]")]
        //[Route("/ap1/v1/auth-google/denied")]
        //[HttpGet("[action]")]
        public IActionResult Denied()
        {
            return Content("You need to allow this application access Google in order to be able to login");
        }

        [HttpGet("[action]")]
        //[Route("/api/v1/auth-google/fail")]
        public IActionResult Fail()
        {
            return Unauthorized();
        }


        [HttpGet("[action]")]
        //[Route("/api/v1/auth-google/name")]
        [Authorize]
        public IActionResult Name()
        {
            var claimsPrincial = (ClaimsPrincipal)User;
            var givenName = claimsPrincial.FindFirst(ClaimTypes.GivenName).Value;
            return Ok(givenName);
        }


        [HttpGet("[action]")]
        //[Route("api/v1/auth-google/isAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            return new ObjectResult(User.Identity.IsAuthenticated);
        }

        /*
        [HttpGet("[action]")]
        public IActionResult SignInWithGoogle(string provider, string returnUrl)
        {
            //var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google",
            //    //Url.Action(nameof(HandleExternalLogin)));
            //    "api/v1/auth-google/HandleExternalLogin");
            //return Challenge(authenticationProperties, "Google");
        }

        //[Route("api/v1/auth-google/HandleExternalLogin")]
        //[Route("external-login", Name = "HandleExternalLogin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> HandleExternalLogin(string returnUrl = null, string remoteError = null,
            CancellationToken cancellationToken)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager
                .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (!result.Succeeded) //user does not exist yet
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var newUser = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                    throw new Exception(createResult.Errors.Select(e => e.Description)
                        .Aggregate((errors, error) => $"{errors}, {error}"));

                await _userManager.AddLoginAsync(newUser, info);
                var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id.ToString()));
                await _userManager.AddClaimsAsync(newUser, newUserClaims);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
            else
            {
                var user = await _userRepository.GetByUsernameAsync(info.Principal.FindFirstValue(ClaimTypes.Email),
                    cancellationToken);

                await _userManager.AddLoginAsync(user, info);
                var userClaims = info.Principal.Claims.Append(new Claim("userId", user.Id.ToString()));
                await _userManager.AddClaimsAsync(user, userClaims);
                await _signInManager.SignInAsync(user, isPersistent: true);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            return Redirect("http://localhost:4200");
        }
        */
    }
}
