using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client/{clientId}/uri/")]
    [Authorize (Roles=Roles.Admin.Name)]
    public class ClientUriController : BaseController
    {

        private readonly IClientService _clientService;

        public ClientUriController(IClientService clientService)
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
            return Ok(client.ToViewModel().Uris);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create(Guid clientId, [FromBody] UpdateClientUriViewModel uri)
        {
            return Ok(_clientService.AddUri(clientId, uri));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(Guid clientId, Guid id, [FromBody] UpdateClientUriViewModel uri)
        {
            _clientService.UpdateUri(clientId, id, uri);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid clientId, Guid id)
        {
            _clientService.RemoveUri(clientId, id);
            return NoContent();
        }
    }
}