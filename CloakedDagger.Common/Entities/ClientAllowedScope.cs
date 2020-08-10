using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ClientAllowedScope : ITrackedEntity
    {
        public Guid ClientAllowedScopeId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public Guid ScopeId { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual Client Client { get; set; }
        
        public virtual Scope Scope { get; set; }
    }
}
