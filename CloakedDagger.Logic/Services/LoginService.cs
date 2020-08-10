using System;
using System.Collections.Generic;
using System.Security.Claims;
using CloakedDagger.Common;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;

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
        private User ValidateLogin(string username, string password)
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
        /// <param name="user"></param>
        /// <returns></returns>
        private ClaimsPrincipal CreateClaimsPrincipalForUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim("sub", user.Username)
                
            };
            var identity = new ClaimsIdentity(claims, "login");
            return new ClaimsPrincipal(identity);
        }
    }
}