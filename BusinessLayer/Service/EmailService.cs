using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmail(string email, string token)
        {
            var resetUrl = $"{_configuration["App:BaseUrl"]}/reset-password?token={token}"; // The reset URL that user clicks to reset the password

            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.Subject = "Password Reset Request";
            message.Body = $"Please reset your password by clicking on the following link: {resetUrl}";
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"])))
            {
                client.Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
                client.EnableSsl = true;

                await client.SendMailAsync(message);
            }
        }
    }
}
