using System;
using CloakedDagger.Common.Enums;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ClientAllowedIdentityEntity : ITrackedEntity
    {
        public Guid ClientAllowedIdentityId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public Identity Identity { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual ClientEntity ClientEntity { get; set; }
    }
}