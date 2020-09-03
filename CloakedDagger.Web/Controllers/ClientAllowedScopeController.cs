using System;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/allowed-scope/")]
    public class ClientAllowedScopeController : BaseController
    {
        private readonly IClientService _clientService;

        public ClientAllowedScopeController(IClientService clientService)
        {
            this._clientService = clientService;
        }
        
        [HttpGet]
        [Route("")]
        private IActionResult GetAll(Guid clientId)
        {
            var client = _clientService.Get(clientId);
            if (null == client)
            {
                return NotFound();
            }

            return Ok(client.AllowedScopes);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] UpdateClientScopeViewModel vm)
        {
            _clientService.AddAllowedScope(clientId, vm.ScopeName);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        public IActionResult Delete(Guid clientId, UpdateClientScopeViewModel vm)
        {
            _clientService.RemoveAllowedScope(clientId, vm.ScopeName);
            return NoContent();
        }
    }
}