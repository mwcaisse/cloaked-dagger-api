using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client")]
    [Authorize (Roles=Roles.Admin.Name)]
    public class ClientController : BaseController
    {

        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            this._clientService = clientService;
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return OkOrNotFound(_clientService.Get(id)?.ToViewModel());
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(_clientService.GetAll());
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] UpdateClientViewModel client)
        {
            return Ok(_clientService.Create(client));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateClientViewModel client)
        {
            return Ok(_clientService.Update(id, client));
        }

        [HttpPost]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _clientService.Activate(id);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/deactivate")]
        public IActionResult Deactivate(Guid id)
        {
            _clientService.Deactivate(id);
            return NoContent();
        }
        
        
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            _clientService.Delete(id);
            return Ok();
        }
        
    }
}