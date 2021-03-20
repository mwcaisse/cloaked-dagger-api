using System;
using System.Collections.Generic;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class UserRegistrationKeyEntity : ITrackedEntity, IActiveEntity
    {
        public Guid Id { get; set; }
        
        public string Key { get; set; }
        
        public int UsesRemaining { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public bool Active { get; set; }
        
        public virtual ICollection<UserRegistrationKeyUseEntity> UserRegirationKeyUses { get; set; }
    }
}