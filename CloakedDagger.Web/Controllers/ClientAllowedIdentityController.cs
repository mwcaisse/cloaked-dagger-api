using System;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Microsoft.AspNetCore.Components.Route("/client/{clientId}/allowed-identity/")]
    public class ClientAllowedIdentityController : BaseController
    {
        private readonly IClientService _clientService;

        public ClientAllowedIdentityController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(Guid clientId)
        {
            var client = _clientService.Get(clientId);
            if (null == client)
            {
                return NotFound();
            }

            return Ok(client.AllowedIdentities);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] UpdateClientAllowedIdentityViewModel vm)
        {
            _clientService.AddAllowedIdentity(clientId, vm.Identity);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        public IActionResult Delete(Guid clientId, [FromBody] UpdateClientAllowedIdentityViewModel vm)
        {
            _clientService.RemoveAllowedIdentity(clientId, vm.Identity);
            return NoContent();
        }
    }
}