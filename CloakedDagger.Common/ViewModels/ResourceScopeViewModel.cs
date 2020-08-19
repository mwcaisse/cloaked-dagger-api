using System;

namespace CloakedDagger.Common.ViewModels
{
    public class ResourceScopeViewModel
    {
        public Guid ResourceScopeId { get; set; }
        
        public Guid ResourceId { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
    }
}