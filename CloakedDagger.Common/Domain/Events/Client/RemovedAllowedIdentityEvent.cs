using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class RemovedAllowedIdentityEvent : ClientDomainEvent
    {
        public Identity Identity { get; set; }
        public override string Type => nameof(RemovedAllowedIdentityEvent);
    }
}