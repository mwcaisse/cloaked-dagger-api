using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientViewModel
    {
        public Guid ClientId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public IEnumerable<ClientUriViewModel> ClientUris { get; set; }
        
        public IEnumerable<ClientAllowedScopeViewModel> ClientAllowedScopes { get; set; }
        
        public IEnumerable<ClientAllowedIdentityViewModel> ClientAllowedIdentities { get; set; }
        
        public IEnumerable<ClientAllowedGrantTypeViewModel> ClientAllowedGrantTypes { get; set; }
    }
}