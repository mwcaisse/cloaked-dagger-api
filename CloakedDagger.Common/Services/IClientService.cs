using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IClientService
    {
        public ClientEntity Get(Guid id);

        public IEnumerable<ClientEntity> GetAll();

        public ClientCreatedViewModel Create(CreateClientViewModel vm);

        public ClientEntity Update(UpdateClientViewModel vm);

        public void Delete(Guid id);

    }
}