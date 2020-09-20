using System;
using System.Collections.Generic;
using CloakedDagger.Common.Domain.Events.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloakedDagger.Common.Converters
{
    public class ClientDomainEventJsonConverter : JsonConverter<ClientDomainEvent>
    {
        public override void WriteJson(JsonWriter writer, ClientDomainEvent value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        private readonly Dictionary<string, Type> _typeMapping = new Dictionary<string, Type>()
        {
            {nameof(AddedAllowedGrantTypeEvent), typeof(AddedAllowedGrantTypeEvent)},
            {nameof(AddedAllowedIdentityEvent), typeof(AddedAllowedIdentityEvent)},
            {nameof(AddedClientUriEvent), typeof(AddedClientUriEvent)},
            {nameof(ClientActivatedEvent), typeof(ClientActivatedEvent)},
            {nameof(ClientCreatedEvent), typeof(ClientCreatedEvent)},
            {nameof(ClientDeactivatedEvent), typeof(ClientDeactivatedEvent)},
            {nameof(ClientRedescribedEvent), typeof(ClientRedescribedEvent)},
            {nameof(ClientRenamedEvent), typeof(ClientRenamedEvent)},
            {nameof(ModifiedClientUriEvent), typeof(ModifiedClientUriEvent)},
            {nameof(RemovedAllowedGrantTypeEvent), typeof(RemovedAllowedGrantTypeEvent)},
            {nameof(RemovedAllowedIdentityEvent), typeof(RemovedAllowedIdentityEvent)},
            {nameof(RemovedClientUriEvent), typeof(RemovedClientUriEvent)},
            {nameof(AddedAllowedScopeEvent), typeof(AddedAllowedScopeEvent)},
            {nameof(RemovedAllowedScopeEvent), typeof(RemovedAllowedScopeEvent)},
        };
        
        public override ClientDomainEvent ReadJson(JsonReader reader, Type objectType, ClientDomainEvent existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var tokens = JToken.Load(reader);
            if (null == tokens["Type"])
            {
                throw new InvalidOperationException("Invalid event object, didn't contain type field");
            }

            var eventType = tokens["Type"].ToString();

            if (_typeMapping.ContainsKey(eventType))
            {
                var e = Activator.CreateInstance(_typeMapping[eventType]);
                serializer.Populate(tokens.CreateReader(), e);
                return e as ClientDomainEvent;
            }

            return new ClientIgnoredEvent();
        }
    }
}