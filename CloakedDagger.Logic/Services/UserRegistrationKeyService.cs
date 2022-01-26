using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using DasCookbook.Common.Exceptions;

namespace CloakedDagger.Logic.Services
{
    public class UserRegistrationKeyService : IUserRegistrationKeyService
    {

        private readonly IUserRegistrationKeyRepository _userRegistrationKeyRepository;

        private readonly IUserRegistrationKeyUseRepository _userRegistrationKeyUseRepository;

        public UserRegistrationKeyService(IUserRegistrationKeyRepository userRegistrationKeyRepository,
            IUserRegistrationKeyUseRepository userRegistrationKeyUseRepository)
        {
            this._userRegistrationKeyRepository = userRegistrationKeyRepository;
            this._userRegistrationKeyUseRepository = userRegistrationKeyUseRepository;
        }
        
        public bool IsValid(string key)
        {
            var entity = _userRegistrationKeyRepository.Get(key);
            return null != entity && entity.Active && entity.UsesRemaining > 0;
        }

        public bool Use(string key, Guid userId)
        {
            if (!IsValid(key))
            {
                return false;
            }

            var entity = _userRegistrationKeyRepository.Get(key);

            _userRegistrationKeyUseRepository.Create(new UserRegistrationKeyUseEntity()
            {
                UserId = userId,
                UserRegistrationKey = entity
            });

            entity.UsesRemaining--;
            _userRegistrationKeyRepository.Update(entity);
            return true;
        }

        public IEnumerable<UserRegistrationKeyEntity> GetAll()
        {
            return _userRegistrationKeyRepository.GetAll();
        }
        
        public UserRegistrationKeyEntity Create(string key, int uses)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new EntityValidationException("Key must not be empty.");
            }

            if (_userRegistrationKeyRepository.Exists(key))
            {
                throw new EntityAlreadyExistsException("A registration key with this key value already exists!");
            }

            if (uses < 1)
            {
                throw new EntityValidationException("Must have at least once use!");
            }

            var entity = new UserRegistrationKeyEntity()
            {
                Active = true,
                Key = key,
                UsesRemaining = uses
            };
            
            _userRegistrationKeyRepository.Create(entity);
            return entity;
        }

        public void Activate(Guid id)
        {
            var urk = GetOrException(id);

            urk.Active = true;
            _userRegistrationKeyRepository.Update(urk);
        }

        public void Deactivate(Guid id)
        {
            var urk = GetOrException(id);

            urk.Active = false;
            _userRegistrationKeyRepository.Update(urk);
        }

        private UserRegistrationKeyEntity GetOrException(Guid id)
        {
            var urk = _userRegistrationKeyRepository.Get(id);
            if (urk == null)
            {
                throw new EntityNotFoundException($"No registration key with the id {id} exists.");
            }

            return urk;
        }
    }
}