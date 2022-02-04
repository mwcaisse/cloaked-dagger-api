using System.Threading.Tasks;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IEmailSender
    {
        public Task<bool> SendEmailAsync(EmailViewModel email);
    }
}