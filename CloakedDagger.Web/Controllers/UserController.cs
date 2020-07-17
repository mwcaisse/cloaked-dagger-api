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

        public UserController(ILoginService loginService)
        {
            this._loginService = loginService;
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
    }
}