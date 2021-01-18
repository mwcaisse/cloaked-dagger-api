using System;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class RoleEntity : ITrackedEntity
    {
        public Guid RoleId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
    }
}