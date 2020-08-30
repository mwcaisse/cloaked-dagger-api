using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class RemovedClientUriEvent : ClientDomainEvent
    {
        public Guid ClientUriId { get; set; }
        public override string Type => nameof(RemovedClientUriEvent);
    }
}