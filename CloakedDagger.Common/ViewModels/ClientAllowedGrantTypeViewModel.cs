using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientAllowedGrantTypeViewModel
    {
        public Guid ClientAllowedGrantTypeId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public ClientGrantType GrantType { get; set; }
    }
}