using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IClientService
    {
        public ClientViewModel Get(Guid id);

        public IEnumerable<ClientViewModel> GetAll();

        public ClientCreatedViewModel Create(UpdateClientViewModel vm);

        public ClientViewModel Update(Guid id, UpdateClientViewModel vm);

        public void Activate(Guid id);

        public void Deactivate(Guid id);

        public void AddAllowedIdentity(Guid id, Identity identity);
        public void RemoveAllowedIdentity(Guid id, Identity identity);

        public void AddAllowedGrantType(Guid id, ClientGrantType grantType);

        public void RemoveAllowedGrantType(Guid id, ClientGrantType grantType);

        public void AddUri(Guid id, UpdateClientUriViewModel vm);

        public void UpdateUri(Guid id, Guid clientUriId, UpdateClientUriViewModel vm);

        public void RemoveUri(Guid id, Guid uriId);

        public void AddAllowedScope(Guid id, string scopeName);

        public void RemoveAllowedScope(Guid id, string scopeName);

        public void Delete(Guid id);

    }
}