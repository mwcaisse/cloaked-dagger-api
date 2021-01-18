using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class UserRoleEntity : ITrackedEntity
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        
        public Guid RoleId { get; set; }
        
        public virtual UserEntity User { get; set; }
        
        public virtual RoleEntity Role { get; set; }

        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
    }
}