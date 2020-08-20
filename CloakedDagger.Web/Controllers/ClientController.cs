using System;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/client")]
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
            return OkOrNotFound(_clientService.Get(id));
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(_clientService.GetAll());
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] CreateClientViewModel client)
        {
            return Ok(_clientService.Create(client));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateClientViewModel client)
        {
            client.ClientId = id;
            return Ok(_clientService.Update(client));
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