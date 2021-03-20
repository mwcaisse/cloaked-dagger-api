using System;
using CloakedDagger.Common.Constants;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class UserRegistrationKeyUseEntity : ITrackedEntity
    {
        public Guid Id { get; set; }
        
        public Guid UserRegistrationKeyId { get; set; }
        
        public Guid UserId { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual UserEntity User { get; set; }
        
        public virtual UserRegistrationKeyEntity UserRegistrationKey { get; set; }
    }
}

