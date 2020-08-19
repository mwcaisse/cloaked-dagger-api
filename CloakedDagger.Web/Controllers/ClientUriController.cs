using System;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/uri/")]
    public class ClientUriController : BaseController
    {

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return NotSupportedYet();
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] ClientUriViewModel uri)
        {
            return NotSupportedYet();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(Guid clientId, Guid id, [FromBody] ClientUriViewModel uri)
        {
            uri.ClientUriId = id;
            uri.ClientId = clientId;
            return NotSupportedYet();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid clientId, Guid id)
        {
            return NotSupportedYet();
        }
    }
}