using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class RemovedAllowedGrantTypeEvent : ClientDomainEvent
    {
        public ClientGrantType GrantType { get; set; }
        public override string Type => nameof(RemovedAllowedGrantTypeEvent);
    }
}