using System;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/allowed-scope/")]
    public class ClientAllowedScopeController : BaseController
    {
        [HttpGet]
        [Route("")]
        private IActionResult GetAll(Guid clientId)
        {
            return NotSupportedYet();
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] CreateClientAllowedScopeViewModel scope)
        {
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