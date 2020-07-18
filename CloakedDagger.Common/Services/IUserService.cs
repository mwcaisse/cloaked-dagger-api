using System;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IUserService
    {
        public UserViewModel Get(Guid id);
        public void Register(UserRegistrationViewModel registration);
    }
}