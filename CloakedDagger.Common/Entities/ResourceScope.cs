using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ResourceScope : ITrackedEntity
    {
        public Guid ResourceScopeId { get; set; }
        
        public Guid ResourceId { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual Resource Resource { get; set; }
        
        public virtual Scope Scope { get; set; }
    }
}