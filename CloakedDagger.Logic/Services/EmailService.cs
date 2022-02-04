using System.Threading.Tasks;
using CloakedDagger.Common.Configuration;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Logic.Services
{
    public class EmailService : IEmailService
    {

        private readonly IEmailSender _emailSender;

        private readonly EmailConfiguration _configuration;
        
        public EmailService(IEmailSender emailSender, EmailConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        private const string EmailVerificationKeyReplacement = "[[verification-key]]";
        
        public async Task<bool> SendEmailVerificationEmail(UserEntity user, string validationKey)
        {
            var emailVerificationUrl =
                _configuration.EmailVerificationUrl.Replace(EmailVerificationKeyReplacement, validationKey);
            
            //TODO: Make the body for this email way less sketchy
            var email = new EmailViewModel()
            {
                To = new EmailAddressViewModel()
                {
                    Name = user.Name,
                    Email = user.Email
                },
                From = new EmailAddressViewModel()
                {
                    Name = _configuration.FromEmailName,
                    Email = _configuration.FromEmailAddress
                },
                Subject = "Verify your email address",
                Body = $"<a href=\"{emailVerificationUrl}\">Click here to verify your email.</a>"
            };

            return await _emailSender.SendEmailAsync(email);
        }
    }
}