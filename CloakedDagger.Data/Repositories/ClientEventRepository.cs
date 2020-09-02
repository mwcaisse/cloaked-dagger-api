using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Domain.Events.Client;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

namespace CloakedDagger.Data.Repositories
{
    public class ClientEventRepository : IClientEventRepository
    {

        private readonly CloakedDaggerDbContext _db;

        public ClientEventRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
            
        public IEnumerable<ClientDomainEvent> GetClientEvents(Guid clientId)
        {
            return _db.ClientEvents.Where(ce => ce.ClientId == clientId)
                .OrderBy(ce => ce.OccurredOn)
                .Select(ce => ce.Event);
        }

        public IDictionary<Guid, IEnumerable<ClientDomainEvent>> GetAllClientEvents()
        {
            return _db.ClientEvents.OrderBy(ce => ce.OccurredOn)
                .GroupBy(ce => ce.ClientId)
                .ToDictionary(g => g.Key, 
                    g => g.Select(ce => ce.Event));
                        

        }

        public void SaveClientEvents(Guid clientId, IEnumerable<ClientDomainEvent> events)
        {
            var entities = events.Select(e => new ClientEventEntity()
            {
                ClientId = clientId,
                Event = e,
                Type = e.Type,
                OccurredOn = e.OccurredOn
            });
            
            _db.ClientEvents.AddRange(entities);
            _db.SaveChanges();
        }
    }
}