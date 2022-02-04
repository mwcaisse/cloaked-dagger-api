using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class UserEmailVerificationRequestEntity : ITrackedEntity, IActiveEntity
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Email { get; set; }
        
        public string ValidationKey { get; set; }
        
        public bool Successful { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public bool Active { get; set; }
        
        public virtual UserEntity User { get; set; }
    }
}