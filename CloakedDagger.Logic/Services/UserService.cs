using System;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;


namespace CloakedDagger.Logic.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;
        
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public UserViewModel Get(Guid id)
        {
            var user = _userRepository.Get(id);
            return null == user ? null : new UserViewModel()
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name, 
            };
        }
        
        public void Register(UserRegistrationViewModel registration)
        {
            ValidateUserRegistration(registration);
            var toCreate = new User()
            {
                Username = registration.Username,
                Email = registration.Email,
                Password = _passwordHasher.HashPassword(registration.Password),
                Name = registration.Name,
                Active = true,
                Locked = true
            };
            _userRepository.Create(toCreate);
        }

        private void ValidateUserRegistration(UserRegistrationViewModel registration)
        {
            ValidationUtils.ValidateViewModel(registration);

            if (_userRepository.UsernameExists(registration.Username))
            {
                throw new EntityValidationException("Username is not available!");
            }
        }
    }
}