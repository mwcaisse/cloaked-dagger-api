using System.Threading.Tasks;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailVerificationEmail(UserEntity user, string validationKey);
    }
}