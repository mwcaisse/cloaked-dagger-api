using System;
using CloakedDagger.Common.Enums;

namespace CloakedDagger.Common.Domain.Events.Client
{
    public class ModifiedClientUriEvent : ClientDomainEvent
    {
        public Guid ClientUriId { get; set; }
        
        public ClientUriType UriType { get; set; }
        
        public string Uri { get; set; }
        public override string Type => nameof(ModifiedClientUriEvent);
    }
}