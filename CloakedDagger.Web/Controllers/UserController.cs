using System.Security.Claims;
using System.Threading.Tasks;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            _loginService = loginService;
            _userService = userService;
        }

        [HttpGet]
        [Route(("me"))]
        [Authorize]
        public IActionResult GetMe()
        {
            return OkOrNotFound(_loginService.GetUserViewModelFromPrincipal(HttpContext.User));
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

            var additionalActionsVm = GetAdditionalActionsForUser(principal);
            if (additionalActionsVm != null)
            {
                return AdditionalActionsRequired(additionalActionsVm);
            }

            return Ok();
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
        [Authorize]
        [Route("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerification()
        {
            await _userService.RequestEmailVerification(GetCurrentUserId());
            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [Route("verify-email")]
        public IActionResult VerifyEmail([FromBody] UserVerifyEmailAddressViewModel vm)
        {
            _userService.ValidateUsersEmail(GetCurrentUserId(), vm.VerificationKey);
            return NoContent();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationViewModel registration)
        {
            await _userService.Register(registration);
            // if registration failed it will throw an EntityValidationException, in which case our middleware will
            //     handle that if we make it here registration was successful so we return 200.
            return Ok(); 
        }
        private UserLoggedInAdditionalActionViewModel GetAdditionalActionsForUser(ClaimsPrincipal principal)
        {
            if (!principal.HasClaim(c => c.Type == UserClaims.EmailVerified))
            {
                return new UserLoggedInAdditionalActionViewModel()
                {
                    EmailVerificationRequired = true
                };
            }

            return null;
        }

        private IActionResult AdditionalActionsRequired(UserLoggedInAdditionalActionViewModel actions)
        {
            return StatusCode(StatusCodes.Status206PartialContent, actions);
        }
    }
}