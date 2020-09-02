namespace CloakedDagger.Common.Domain.Events.Client
{
    public class RemovedAllowedScope : ClientDomainEvent
    {
        public string ScopeName { get; set; }
        public override string Type => nameof(RemovedAllowedScope);
    }
}