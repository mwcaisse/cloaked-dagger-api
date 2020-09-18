using System;
using System.Collections.Generic;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ClientEntity : ITrackedEntity, IActiveEntity
    {
        public Guid ClientId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Secret { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public bool Active { get; set; }

        public virtual ICollection<ClientUriEntity> ClientUris { get; set; }
        
        public virtual ICollection<ClientAllowedScopeEntity> ClientAllowedScopes { get; set; }
        
        public virtual ICollection<ClientAllowedIdentityEntity> ClientAllowedIdentities { get; set; }
        
        public virtual ICollection<ClientAllowedGrantTypeEntity> ClientAllowedGrantTypes { get; set; }
    }
}