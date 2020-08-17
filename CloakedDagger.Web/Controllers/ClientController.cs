using System;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client")]
    public class ClientController : BaseController
    {

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return NotSupportedYet();
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return NotSupportedYet();
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] ClientViewModel client)
        {
            return NotSupportedYet();
        }

        [HttpPut]
        [Route("{id")]
        public IActionResult Update(Guid id, [FromBody] ClientViewModel client)
        {
            client.ClientId = id;
            return NotSupportedYet();
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            return NotSupportedYet();
        }
        
    }
}