using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    
    [Route("/client/{clientId}/allowed-grant-type")]
    [Authorize (Roles=Roles.Admin.Name)]
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

            return Ok(client.ToViewModel().AllowedGrantTypes);
        }
        
        [HttpPost]
        [Route("{grantType:int}")]
        public IActionResult Create(Guid clientId, ClientGrantType grantType)
        {
            _clientService.AddAllowedGrantType(clientId, grantType);
            return NoContent();
        }

        [HttpDelete]
        [Route("{grantType:int}")]
        public IActionResult Delete(Guid clientId, ClientGrantType grantType)
        {
            _clientService.RemoveAllowedGrantType(clientId, grantType);
            return NoContent();
        }
    }
}