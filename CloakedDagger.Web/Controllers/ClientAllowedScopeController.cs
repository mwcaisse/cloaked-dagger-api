using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/allowed-scope/")]
    [Authorize (Roles=Roles.Admin.Name)]
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

            return Ok(client.ToViewModel().AllowedScopes);
        }

        [HttpPost]
        [Route("{scopeName}")]
        public IActionResult Create(Guid clientId, string scopeName)
        {
            _clientService.AddAllowedScope(clientId, scopeName);
            return NoContent();
        }

        [HttpDelete]
        [Route("{scopeName}")]
        public IActionResult Delete(Guid clientId, string scopeName)
        {
            _clientService.RemoveAllowedScope(clientId, scopeName);
            return NoContent();
        }
    }
}