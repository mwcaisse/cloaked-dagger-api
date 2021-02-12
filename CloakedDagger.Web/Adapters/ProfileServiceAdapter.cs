using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CloakedDagger.Common.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace CloakedDagger.Web.Adapters
{
    public class ProfileServiceAdapter : IProfileService
    {

        private readonly IUserService _userService;

        public ProfileServiceAdapter(IUserService userService)
        {
            this._userService = userService;
        }


        private readonly HashSet<string> _profileClaimTypes = new()
        {
            "name",
            "username",
            "id",
            "role"
        };
        
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claimsToAdd = context.Subject.Claims.Where(c => _profileClaimTypes.Contains(c.Type));
            context.IssuedClaims.AddRange(claimsToAdd);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var userIdClaim = context.Subject
                .FindAll(c => c.Type == ClaimTypes.Sid || c.Type == "sub")
                .FirstOrDefault();
            
            if (null != userIdClaim && !string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                context.IsActive = _userService.IsActive(Guid.Parse(userIdClaim.Value));
            }
            else
            {
                context.IsActive = false;
            }
            return Task.CompletedTask;
        }
    }
}