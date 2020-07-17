using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/api/test/values")]
    public class ValuesController : Controller
    {

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(new List<string>()
            {
                "Value 1",
                "Value 2"
            });
        }
        
    }
}