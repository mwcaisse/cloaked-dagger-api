using System;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;

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
    }
}