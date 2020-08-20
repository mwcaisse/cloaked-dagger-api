using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientAllowedIdentityViewModel
    {
        public Guid ClientAllowedIdentityId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public Identity Identity { get; set; }
    }
}