using System;
using System.Collections.Generic;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class UserEntity : ITrackedEntity, IActiveEntity
    {
        
        public Guid UserId { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public bool Active { get; set; }
        
        public bool Locked { get; set; }

        // TOOD: Make this a saved field, but for now assume its always true
        public bool RequiresTfa => true;

        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual ICollection<UserRoleEntity> Roles { get; set; }

    }
}