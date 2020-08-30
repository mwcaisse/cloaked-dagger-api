using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Data.Extensions;
using OwlTin.Common;

namespace CloakedDagger.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public ClientRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public ClientEntity Get(Guid id)
        {
            return _db.Clients.Build().FirstOrDefault(c => c.ClientId == id);
        }

        public bool ClientWithNameExists(string name, Guid? excludeClientId = null)
        {
            var query = _db.Clients.Where(c => c.Name == name);

            if (excludeClientId.HasValue)
            {
                query = query.Where(c => c.ClientId != excludeClientId);
            }

            return query.Any();
        }

        public IEnumerable<ClientEntity> GetAll()
        {
            return _db.Clients.Build();
        }

        public ClientEntity Create(ClientEntity clientEntity)
        {
            _db.Add(clientEntity);
            _db.SaveChanges();
            return clientEntity;
        }

        public ClientEntity Update(ClientEntity clientEntity)
        {
            _db.Attach(clientEntity);
            _db.SaveChanges();
            return clientEntity;
        }

        public void Delete(Guid id)
        {
            var client = _db.Clients.FirstOrDefault(c => c.ClientId == id);
            if (null != client)
            {
                _db.Clients.Remove(client);
                _db.SaveChanges();
            }
        }
    }
}