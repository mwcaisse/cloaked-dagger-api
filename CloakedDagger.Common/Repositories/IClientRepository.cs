using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IClientRepository
    {
        public Client Get(Guid id);

        public bool ClientWithNameExists(string name, Guid? excludeClientId = null);

        public IEnumerable<Client> GetAll();

        public Client Create(Client client);

        public Client Update(Client client);

        public void Delete(Guid id);
    }
}