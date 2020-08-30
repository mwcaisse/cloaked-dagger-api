using System;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientRenamedEvent : ClientDomainEvent
    {
        public string Name { get; set; }

        public override string Type => nameof(ClientRenamedEvent);
    }
}