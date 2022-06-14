using MailKit.Net.Smtp;
using MimeKit;

namespace ECommerce.Utility
{
    public class EmailSender : IEmailSender
    {
        private SmtpConfiguration _smtpConfiguration;

        public EmailSender(SmtpConfiguration smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration;
        }
        public async Task SendAsync(string subject, string body, string receiverEmail, string receiverName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpConfiguration.SenderName,
                _smtpConfiguration.SenderEmail));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.Server, _smtpConfiguration.Port,
                    _smtpConfiguration.UseSSL);
                await client.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
