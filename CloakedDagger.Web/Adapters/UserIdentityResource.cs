using System.Collections.Generic;
using System.Formats.Asn1;
using IdentityServer4.Models;

namespace CloakedDagger.Web.Adapters
{
    public class UserIdentityResource : IdentityResource
    {

        public const string Scope = "user";
        
        private static readonly List<string> _claims = new List<string>()
        {
            "id",
            "username",
            "name",
            "role"
        };
        
        public UserIdentityResource()
        {
            Name = Scope;
            DisplayName = "User Information";
            Description = "Basic user information";
            Emphasize = true;
            UserClaims = _claims;
        }
        
        
    }
}