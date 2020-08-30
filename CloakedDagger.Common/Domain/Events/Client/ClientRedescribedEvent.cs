using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientRedescribedEvent : ClientDomainEvent
    {
        public string Description { get; set; }

        public override string Type => nameof(ClientRedescribedEvent);
    }
}