using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace HotelApp.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.example.com") // Thay bằng máy chủ SMTP thực tế
            {
                Port = 587, // Cổng SMTP (thường là 587 cho SSL)
                Credentials = new NetworkCredential("your_email@example.com", "your_password"), // Thay bằng thông tin email thực tế
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your_email@example.com"), // Địa chỉ email người gửi
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true, // Đặt là true nếu bạn gửi HTML
            };

            mailMessage.To.Add(email); // Thêm địa chỉ email người nhận

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}
