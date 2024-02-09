using System.Net.Mail;

namespace evanairlines.Interfaces

{
    public interface IEmailService
    {
        void SendPasswordResetEmail(string email, string resetLink);
    }

    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient smtpClient;

        public SmtpEmailService(SmtpClient _smtpClient)
        {
            smtpClient = _smtpClient;
        }

        public void SendPasswordResetEmail(string email, string resetLink)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("johnsonjevane@hotmail.com"),
                Subject = "Password Reset",
                Body = $"Click the following link to reset your password: {resetLink}",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }
    }
}
