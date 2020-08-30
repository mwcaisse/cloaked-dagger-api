using System;
using CloakedDagger.Common.Enums;
using OwlTin.Common.Entities;

namespace CloakedDagger.Common.Entities
{
    public class ClientUriEntity : ITrackedEntity
    {
        public Guid ClientUriId { get; set; }
        
        public Guid ClientId { get; set; }
        
        public ClientUriType Type { get; set; }
        
        public string Uri { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
        
        public virtual ClientEntity ClientEntity { get; set; }
    }
}