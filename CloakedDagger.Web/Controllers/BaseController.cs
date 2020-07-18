using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        ///  Returns the Id of the currently logged in user. Will throw an exception if there is no logged in user.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">If there is no logged in user or couldn't find the user's id</exception>
        public Guid GetCurrentUserId()
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
            if (null != userIdClaim && !string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                return Guid.Parse(userIdClaim.Value);
            }

            throw new Exception("No currently logged in user!");
        }
        
    }
}