namespace CloakedDagger.Common.Domain.Events.Client
{
    public class AddedAllowedScope : ClientDomainEvent
    {
        public string ScopeName { get; set; }
        public override string Type => nameof(AddedAllowedScope);
    }
}