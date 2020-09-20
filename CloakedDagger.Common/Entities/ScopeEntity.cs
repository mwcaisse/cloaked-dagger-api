using System;
using System.Collections.Generic;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ScopeEntity : ITrackedEntity, IActiveEntity
    {
        public Guid ScopeId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public bool Active { get; set; }
        
        public virtual ICollection<ResourceScopeEntity> ResourceScopes { get; set; }
    }
}