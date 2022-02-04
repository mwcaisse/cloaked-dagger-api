using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/resource")]
    [Authorize (Roles=Roles.Admin.Name)]
    public class ResourceController : BaseController
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            this._resourceService = resourceService;
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return OkOrNotFound(_resourceService.Get(id));
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(_resourceService.GetAll());
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] ResourceViewModel resource)
        {
            return Ok(_resourceService.Create(resource));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(Guid id, [FromBody] ResourceViewModel resource)
        {
            resource.ResourceId = id;
            return Ok(_resourceService.Update(resource));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            _resourceService.Delete(id);
            return Ok();
        }
        
    }
}