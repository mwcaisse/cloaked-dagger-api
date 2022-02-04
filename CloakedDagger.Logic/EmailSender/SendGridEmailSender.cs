using System.Threading.Tasks;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CloakedDagger.Logic.EmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient _client;

        public SendGridEmailSender(SendGridConfiguration config)
        {
            _client = new SendGridClient(config.ApiKey);
        }
        
        public async Task<bool> SendEmailAsync(EmailViewModel email)
        {
            var message = MailHelper.CreateSingleEmail(new EmailAddress(email.From.Email, email.From.Name),
                new EmailAddress(email.To.Email, email.From.Name), email.Subject, null, email.Body);

            var response = await _client.SendEmailAsync(message);

            return response.IsSuccessStatusCode;
        }
    }
}