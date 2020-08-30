using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ResourceScopeEntity : ITrackedEntity
    {
        public Guid ResourceScopeId { get; set; }
        
        public Guid ResourceId { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual ResourceEntity ResourceEntity { get; set; }
        
        public virtual ScopeEntity ScopeEntity { get; set; }
    }
}