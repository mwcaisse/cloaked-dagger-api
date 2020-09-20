namespace CloakedDagger.Common.Domain.Events.Client
{
    public class RemovedAllowedScopeEvent : ClientDomainEvent
    {
        public string ScopeName { get; set; }
        public override string Type => nameof(RemovedAllowedScopeEvent);
    }
}