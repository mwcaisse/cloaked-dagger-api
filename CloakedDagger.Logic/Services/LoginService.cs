using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CloakedDagger.Common;
using CloakedDagger.Common.Constants;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Logic.Services
{
    public class LoginService : ILoginService
    {

        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;

        public LoginService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }
        
        public ClaimsPrincipal Login(string username, string password)
        {
            var user = ValidateLogin(username, password);
            if (null == user)
            {
                return null;
            }

            return CreateClaimsPrincipalForUser(user);
        }

        /// <summary>
        ///  Attempts to login a user with the given username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>The user object if login was successful, null if the login failed</returns>
        private UserEntity ValidateLogin(string username, string password)
        {
            var user = _userRepository.Get(username);
            if (null == user)
            {
                //We still want to run this if the user wasn't found, that way it still takes a similar
                //    amount of time if user wasn't found as if it was, to prevent timing attacks
                _passwordHasher.VerifyPassword(Guid.NewGuid().ToString(), password);
                return null;
            }

            if (!_passwordHasher.VerifyPassword(user.Password, password))
            {
                //Invalid password, abort
                return null;
            }

            return user;
        }

        /// <summary>
        ///  Creates a ClaimsPrincipal for authentication that represents the given user
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        private ClaimsPrincipal CreateClaimsPrincipalForUser(UserEntity userEntity)
        {
            var identity = new ClaimsIdentity(GetClaimsForUser(userEntity), "login");
            return new ClaimsPrincipal(identity);
        }
        
        public static List<Claim> GetClaimsForUser(UserEntity userEntity)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userEntity.Username),
                new(ClaimTypes.Sid, userEntity.UserId.ToString()),
                new(UserClaims.Id, userEntity.UserId.ToString()),
                new(UserClaims.Username, userEntity.Username),
                new(UserClaims.Name, userEntity.Name)
            };
            if (userEntity.EmailVerified)
            {
                claims.Add(new Claim(UserClaims.EmailVerified, userEntity.EmailVerified.ToString()));
                // Identity Server requires the sub field be on the user to identity them. Don't add it until they are
                //  fully authenticated / have their email verified
                claims.Add(new(UserClaims.Subject, userEntity.UserId.ToString()));
                
                   
                foreach (var roleEntity in userEntity.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleEntity.Role.Name));
                    claims.Add(new Claim(UserClaims.Role, roleEntity.Role.Name));
                }
            }
         
            return claims;
        }

        public UserViewModel GetUserViewModelFromPrincipal(ClaimsPrincipal user)
        {
            if (!user.HasClaim(c => c.Type == UserClaims.Id))
            {
                return null;
            }
            
            var vm = new UserViewModel()
            {
                Id = Guid.Parse(user.FindFirst(UserClaims.Id)?.Value),
                Username = user.FindFirst(UserClaims.Username)?.Value,
                Name = user.FindFirst(UserClaims.Name)?.Value,
                Roles = user.FindAll(UserClaims.Role).Select(c => c.Value).ToList()
            };
            if (!user.HasClaim(c => c.Type == UserClaims.EmailVerified))
            {
                vm.AdditionalActions = new UserLoggedInAdditionalActionViewModel()
                {
                    EmailVerificationRequired = true
                };
            }
            return vm;
        }
    }
}