using System;
using System.Threading.Tasks;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IUserService
    {
        UserViewModel Get(Guid id);

        bool IsActive(Guid id);
        Task Register(UserRegistrationViewModel registration);

        Task RequestEmailVerification(Guid userId);

        void ValidateUsersEmail(Guid userId, string emailVerificationKey);
    }
}