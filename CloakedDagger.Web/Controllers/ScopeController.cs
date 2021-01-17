
using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{

    [Route("/scope")]
    [Authorize]
    public class ScopeController : BaseController
    {

        private readonly IResourceScopeService _resourceScopeService;
        
        public ScopeController(IResourceScopeService resourceScopeService)
        {
            this._resourceScopeService = resourceScopeService;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string text)
        {
            return Ok(_resourceScopeService.Search(text));
        }
    }
}