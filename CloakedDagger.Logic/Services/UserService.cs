using System;
using System.Linq;
using System.Threading.Tasks;
using CloakedDagger.Common;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using DasCookbook.Common.Exceptions;

namespace CloakedDagger.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IUserRegistrationKeyService _userRegistrationKeyService;

        private readonly IUserRoleRepository _userRoleRepository;

        private readonly IUserEmailVerificationRequestRepository _userEmailVerificationRequestRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository, IPasswordHasher passwordHasher,
            IUserRegistrationKeyService userRegistrationKeyService, 
            IUserEmailVerificationRequestRepository userEmailVerificationRequestRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _passwordHasher = passwordHasher;
            _userRegistrationKeyService = userRegistrationKeyService;
            _userEmailVerificationRequestRepository = userEmailVerificationRequestRepository;
            _emailService = emailService;
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

        public bool IsActive(Guid id)
        {
            var user = _userRepository.Get(id);
            return null != user && user.Locked == false;
        }

        public async Task Register(UserRegistrationViewModel registration)
        {
            ValidateUserRegistration(registration);
            var toCreate = new UserEntity()
            {
                Username = registration.Username,
                Email = registration.Email,
                Password = _passwordHasher.HashPassword(registration.Password),
                Name = registration.Name,
                Active = true,
                Locked = false
            };
            _userRepository.Create(toCreate);

            var registrationKeyValid = false;
            try
            {
                registrationKeyValid = _userRegistrationKeyService.Use(registration.RegistrationKey, toCreate.UserId);
            }
            finally
            {
                if (!registrationKeyValid)
                {
                    _userRepository.Delete(toCreate.UserId);
                }
            }
            AddRole(toCreate.UserId, Roles.User.Id);
            await CreateEmailVerificationRequest(toCreate);
        }

        private void AddRole(Guid id, Guid roleId)
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

            if (!_userRegistrationKeyService.IsValid(registration.RegistrationKey))
            {
                throw new EntityValidationException("Registration Key is not valid!");
            }
        }

        /// <summary>
        /// Creates a new Email verification request for the given user
        /// </summary>
        /// <param name="user"></param>
        private async Task CreateEmailVerificationRequest(UserEntity user)
        {
            // get any current verification requests for the user, and mark them as unsucessful and inactive
            var currentRequests =_userEmailVerificationRequestRepository.GetActiveForUser(user.UserId).ToList();
            var mostRecentRequest = currentRequests.OrderByDescending(x => x.CreateDate).FirstOrDefault();
            if (mostRecentRequest != null)
            {
                //TODO: Currently dates in entities are in server's timestamp not UTC
                if (mostRecentRequest.CreateDate + TimeSpan.FromMinutes(10) > DateTime.Now)
                {
                    throw new EntityValidationException(
                        "We recently sent you an email verification request. Please wait a bit before re-requesting.");
                }
            }
            
            foreach (var request in currentRequests)
            {
                request.Active = false;
                request.Successful = false;
                
                _userEmailVerificationRequestRepository.Update(request);
            }

            var validationRequest = new UserEmailVerificationRequestEntity()
            {
                ValidationKey = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                Email = user.Email,
                Successful = false,
                Active = true
            };
            _userEmailVerificationRequestRepository.Create(validationRequest);

            var success = await _emailService.SendEmailVerificationEmail(user, validationRequest.ValidationKey);
            if (!success)
            {
                validationRequest.Active = false;
                validationRequest.Successful = false;
                _userEmailVerificationRequestRepository.Update(validationRequest);

                throw new EntityValidationException("Unable to send verification email.");
            }
        }

        public void ValidateUsersEmail(Guid userId, string emailVerificationKey)
        {
            var user = _userRepository.Get(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"No user with id {userId} exists!");
            }

            var validationKey = _userEmailVerificationRequestRepository.GetActiveByKey(emailVerificationKey);

            if (validationKey == null || validationKey.UserId != userId || validationKey.Email != user.Email)
            {
                throw new EntityValidationException("This validation key is not valid for the given user");
            }

            validationKey.Successful = true;
            validationKey.Active = false;
            _userEmailVerificationRequestRepository.Update(validationKey);

            user.EmailVerified = true;
            _userRepository.Update(user);
        }

        public async Task RequestEmailVerification(Guid userId)
        {
            var user = _userRepository.Get(userId);
            await CreateEmailVerificationRequest(user);
        }
    }
}