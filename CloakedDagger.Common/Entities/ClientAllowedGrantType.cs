using System;
using CloakedDagger.Common.Enums;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ClientAllowedGrantType : ITrackedEntity
    {
        public Guid ClientAllowedGrantTypeId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public ClientGrantType GrantType { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual Client Client { get; set; }
    }
}