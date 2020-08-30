using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IClientRepository
    {
        public ClientEntity Get(Guid id);

        public bool ClientWithNameExists(string name, Guid? excludeClientId = null);

        public IEnumerable<ClientEntity> GetAll();

        public ClientEntity Create(ClientEntity clientEntity);

        public ClientEntity Update(ClientEntity clientEntity);

        public void Delete(Guid id);
    }
}