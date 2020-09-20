namespace CloakedDagger.Common.Domain.Events.Client
{
    public class AddedAllowedScopeEvent : ClientDomainEvent
    {
        public string ScopeName { get; set; }
        public override string Type => nameof(AddedAllowedScopeEvent);
    }
}