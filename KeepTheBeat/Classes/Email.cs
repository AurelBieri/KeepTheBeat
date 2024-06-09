using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace KeepTheBeat.Classes
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _recipientEmail;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpUser = configuration["Email:SmtpUser"];
            _smtpPass = configuration["Email:SmtpPass"];
            _recipientEmail = configuration["Email:RecipientEmail"];
        }

        public async Task SendEmailAsync(string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Keep The Beat", _smtpUser));
            message.To.Add(new MailboxAddress("Admin", _recipientEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
