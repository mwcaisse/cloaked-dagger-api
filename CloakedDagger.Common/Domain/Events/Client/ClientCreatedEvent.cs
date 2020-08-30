using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientCreatedEvent : ClientDomainEvent
    {
        public string Name { get; set; }
        
        public string Secret { get; set; }
        
        public string Description { get; set; }

        public override string Type => nameof(ClientCreatedEvent);
    }
}