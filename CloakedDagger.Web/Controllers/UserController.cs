using System.Security.Claims;
using System.Threading.Tasks;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Web.Constants;
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

            await SignInAsync(principal);
            
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
            //TODO: We need to sign out of all schemes we are logged in as
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CloakedDaggerAuthenticationSchemes.Partial)]
        [Route("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerification()
        {
            await _userService.RequestEmailVerification(GetCurrentUserId());
            return NoContent();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CloakedDaggerAuthenticationSchemes.Partial)]
        [Route("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] UserVerifyEmailAddressViewModel vm)
        {
            var currentUserId = GetCurrentUserId();
            var emailVerified = _userService.ValidateUsersEmail(currentUserId, vm.VerificationKey);

            if (emailVerified)
            {
                // if their email was verified successfully, re-log in with the appropriate claims / scheme
                var fullPrincipal = _loginService.RefreshClaimsPrincipalForUser(currentUserId);
                await SignInAsync(fullPrincipal);
                return NoContent();
            }

            // We won't actually ever hit this, since emailVerified shouldn't ever return false, and throw exceptions
            //  if it fails, but this feels safer.
            return BadRequest();
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

        private async Task SignInAsync(ClaimsPrincipal principal)
        {
            if (!principal.HasClaim(c => c.Type == UserClaims.EmailVerified))
            {
                // if their email isn't verified, log in with the partial scheme
                await HttpContext.SignInAsync(CloakedDaggerAuthenticationSchemes.Partial, principal);
            }
            else
            {
                // otherwise, they are fulled authorized, use default scheme
                await HttpContext.SignInAsync(CloakedDaggerAuthenticationSchemes.Default, principal);
            }
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