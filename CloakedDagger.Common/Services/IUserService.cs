using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IUserService
    {
        public void Register(UserRegistrationViewModel registration);
    }
}