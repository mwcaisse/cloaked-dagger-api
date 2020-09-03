using System;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    
    [Microsoft.AspNetCore.Components.Route("/client/{clientId}/allowed-grant-type/")]
    public class ClientAllowedGrantTypeController : BaseController
    {

        private readonly IClientService _clientService;

        public ClientAllowedGrantTypeController(IClientService clientService)
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

            return Ok(client.AllowedGrantTypes);
        }
        
        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] UpdateClientAllowedGrantTypeViewModel vm)
        {
            _clientService.AddAllowedGrantType(clientId, vm.Type);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        public IActionResult Delete(Guid clientId, [FromBody] UpdateClientAllowedGrantTypeViewModel vm)
        {
            _clientService.RemoveAllowedGrantType(clientId, vm.Type);
            return NoContent();
        }
        
        

    }
}