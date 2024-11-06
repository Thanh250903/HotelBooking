using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using HotelApp.Models.User;

namespace HotelApp.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly SendEmail _emailSettings;

        public EmailSender(IOptions<SendEmail> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email address cannot be null or empty");
            }

            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = $"<p href='https://localhost:7257/Home/ConfirmEmail?email={email}'>{htmlMessage}</p>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
       