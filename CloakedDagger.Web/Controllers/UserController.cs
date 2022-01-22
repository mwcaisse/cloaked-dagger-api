using System;
using System.Threading.Tasks;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/user")]
    public class UserController : BaseController
    {

        private readonly ILoginService _loginService;

        private readonly IUserService _userService;

        public UserController(ILoginService loginService, IUserService userService)
        {
            this._loginService = loginService;
            this._userService = userService;
        }

        [HttpGet]
        [Route(("me"))]
        [Authorize]
        public IActionResult GetMe()
        {
            return Ok(_userService.Get(GetCurrentUserId()));
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel user)
        {
            var principal = _loginService.Login(user.Username, user.Password);
            if (null == principal)
            {
                return Unauthorized("Invalid credentials provided.");
            }

            await HttpContext.SignInAsync(principal);
            return Ok();
        }

        [HttpPost]
        [Route("login/mfa")]
        public async Task<IActionResult> LoginValidateMfa([FromBody] UserLoginMfaViewModel mfa)
        {
            /*
             * Helpful links
             *
             *  https://docs.microsoft.com/en-us/aspnet/core/security/authentication/mfa?view=aspnetcore-5.0
             *      -- Name the RequireClaim portion? This could be helpful to say its not authenticated unclass
             *              the given claim exists / is present?
             * 
             */
            return Unauthorized();
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserRegistrationViewModel registration)
        {
            _userService.Register(registration);
            // if registration failed it will throw an EntityValidationException, in which case our middleware will
            //     handle that if we make it here registration was successful so we return 200.
            return Ok(); 
        }
    }
}