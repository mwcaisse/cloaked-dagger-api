using System;
using CloakedDagger.Common.Domain.Events.Client;

namespace CloakedDagger.Common.Entities
{
    public class ClientEventEntity
    {
        public Guid Id { get; set; }
        
        public Guid ClientId { get; set; }
        
        public DateTime OccurredOn { get; set; }
        
        public string Type { get; set; }
        
        public ClientDomainEvent Event { get; set; }
    }
}