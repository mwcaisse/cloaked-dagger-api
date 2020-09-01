using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientIgnoredEvent : ClientDomainEvent
    {
        public new DateTime OccurredOn => DateTime.UtcNow;
        
        public new Guid ClientId => Guid.NewGuid();
        public override string Type => nameof(ClientIgnoredEvent);
    }
}