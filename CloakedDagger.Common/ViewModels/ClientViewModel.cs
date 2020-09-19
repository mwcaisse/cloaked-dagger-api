using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool Active { get; set; }
        
        public IEnumerable<ClientUriViewModel> Uris { get; set; }
        
        public IEnumerable<string> AllowedScopes { get; set; }
        
        public IEnumerable<Identity> AllowedIdentities { get; set; }
        
        public IEnumerable<ClientGrantType> AllowedGrantTypes { get; set; }
    }
}