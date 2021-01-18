using System;
using System.Linq;
using CloakedDagger.Common;
using CloakedDagger.Common.Constants;
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

        private readonly IRoleRepository _roleRepository;

        private readonly IUserRoleRepository _userRoleRepository;

        private readonly IPasswordHasher _passwordHasher;
        
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository, IPasswordHasher passwordHasher)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._userRoleRepository = userRoleRepository;
            this._passwordHasher = passwordHasher;
        }

        public UserViewModel Get(Guid id)
        {
            var user = _userRepository.Get(id);
            return null == user ? null : new UserViewModel()
            {
                Id = user.UserId,
                Username = user.Username,
                Name = user.Name, 
                Roles = user.Roles.Select(ur => ur.Role.Name).ToList()
            };
        }
        
        public void Register(UserRegistrationViewModel registration)
        {
            ValidateUserRegistration(registration);
            var toCreate = new UserEntity()
            {
                Username = registration.Username,
                Email = registration.Email,
                Password = _passwordHasher.HashPassword(registration.Password),
                Name = registration.Name,
                Active = true,
                Locked = true
            };
            _userRepository.Create(toCreate);
            AddRole(toCreate.UserId, Roles.User.Id);
        }

        public void AddRole(Guid id, Guid roleId)
        {
            if (null == _userRepository.Get(id))
            {
                throw new EntityValidationException("User does not exist.");
            }

            if (null == _roleRepository.Get(roleId))
            {
                throw new EntityValidationException("Role does not exist.");
            }

            _userRoleRepository.Create(id, roleId);
        }

        public void RemoveRole(Guid id, Guid roleId)
        {
            if (null == _userRepository.Get(id))
            {
                throw new EntityValidationException("User does not exist.");
            }

            if (null == _roleRepository.Get(roleId))
            {
                throw new EntityValidationException("Role does not exist.");
            }

            _userRoleRepository.Delete(id, roleId);
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