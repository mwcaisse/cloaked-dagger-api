using System;
using System.Linq;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloakedDagger.Web.Controllers
{
    [Route("/user/registration-key")]
    [Authorize (Roles=Roles.Admin.Name)]
    public class RegistrationKeyController : BaseController
    {

        private readonly IUserRegistrationKeyService _userRegistrationKeyService;

        public RegistrationKeyController(IUserRegistrationKeyService userRegistrationKeyService)
        {
            _userRegistrationKeyService = userRegistrationKeyService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(_userRegistrationKeyService.GetAll());
        }

        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody] CreateUserRegistrationKeyViewModel vm)
        {
            var created = _userRegistrationKeyService.Create(vm.Key, vm.Uses);
            return Ok(created);
        }

        [HttpPost]
        [Route("{id}/activate")]
        public IActionResult Activate(Guid id)
        {
            _userRegistrationKeyService.Activate(id);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/deactivate")]
        public IActionResult Deactivate(Guid id)
        {
            _userRegistrationKeyService.Deactivate(id);
            return Ok();
        }
    }
}