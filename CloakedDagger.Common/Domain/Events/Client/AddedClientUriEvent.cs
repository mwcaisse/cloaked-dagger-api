using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class AddedClientUriEvent : ClientDomainEvent
    {
        public Guid ClientUriId { get; set; }
        
        public ClientUriType UriType { get; set; }
        
        public string Uri { get; set; }
        
        public override string Type => nameof(AddedClientUriEvent);
    }
}