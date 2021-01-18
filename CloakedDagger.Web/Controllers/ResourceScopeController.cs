using System;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/resource/{resourceId}/scope/")]
    [Authorize (Roles=Roles.Admin.Name)]
    public class ResourceScopeController : BaseController
    {

        private readonly IResourceScopeService _resourceScopeService;

        public ResourceScopeController(IResourceScopeService resourceScopeService)
        {
            this._resourceScopeService = resourceScopeService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(Guid resourceId)
        {
            return Ok(_resourceScopeService.GetForResource(resourceId));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            return OkOrNotFound(_resourceScopeService.Get(id));
        }

        [HttpPost]
        [Route("")]
        public IActionResult Add(Guid resourceId, [FromBody] AddResourceScopeViewModel scope)
        {
            scope.ResourceId = resourceId;
            return Ok(_resourceScopeService.Create(scope));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Remove(Guid id)
        {
            _resourceScopeService.Delete(id);
            return Ok();
        }
        
    }
}