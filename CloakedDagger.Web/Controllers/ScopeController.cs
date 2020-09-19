using CloakedDagger.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/scope")]
    public class ScopeController : BaseController
    {

        private readonly IResourceScopeService _resourceScopeService;
        
        public ScopeController(IResourceScopeService resourceScopeService)
        {
            this._resourceScopeService = resourceScopeService;
        }

        [Route("/search")]
        public IActionResult Search([FromQuery]string text)
        {
            return Ok(_resourceScopeService.Search(text));
        }
    }
}