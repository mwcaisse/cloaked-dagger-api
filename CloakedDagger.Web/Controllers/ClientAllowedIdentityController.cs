using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/allowed-identity")]
    [Authorize (Roles=Roles.Admin.Name)]
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

            return Ok(client.ToViewModel().AllowedIdentities);
        }

        [HttpPost]
        [Route("{identity}")]
        public IActionResult Create(Guid clientId, Identity identity)
        {
            _clientService.AddAllowedIdentity(clientId, identity);
            return NoContent();
        }

        [HttpDelete]
        [Route("{identity}")]
        public IActionResult Delete(Guid clientId, Identity identity)
        {
            _clientService.RemoveAllowedIdentity(clientId, identity);
            return NoContent();
        }
    }
}