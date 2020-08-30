using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public abstract class ClientDomainEvent : DomainEvent
    {
        public Guid ClientId { get; set;  }
    }
}