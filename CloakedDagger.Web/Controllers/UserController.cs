using System.Threading.Tasks;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {

        private readonly ILoginService _loginService;

        private readonly IUserService _userService;

        public UserController(ILoginService loginService, IUserService userService)
        {
            this._loginService = loginService;
            this._userService = userService;
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