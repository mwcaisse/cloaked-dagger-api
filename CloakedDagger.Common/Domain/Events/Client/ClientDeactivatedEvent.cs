namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ClientDeactivatedEvent : ClientDomainEvent
    {
        public override string Type => nameof(ClientDeactivatedEvent);
    }
}