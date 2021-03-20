using System;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserRegistrationKeyUseRepository
    {
        public void Create(UserRegistrationKeyUseEntity entity);
    }
}