using System;

namespace CloakedDagger.Common.ViewModels
{
    public class ClientAllowedScopeViewModel
    {
        public Guid ClientAllowedScopeId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public string ScopeName { get; set; }
    }
}