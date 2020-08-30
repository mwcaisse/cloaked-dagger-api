using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientActivatedEvent : ClientDomainEvent
    {
        public override string Type => nameof(ClientActivatedEvent);
    }
}