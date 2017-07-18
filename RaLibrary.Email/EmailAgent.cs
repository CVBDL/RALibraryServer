using System;
using System.Net.Mail;
using System.Net;

namespace RaLibrary.Email
{
    /// <summary>
    /// Singleton
    /// </summary>
    public class EmailAgent
    {
        private EmailAgent() { }

        private static EmailAgent instance;
        public static EmailAgent Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmailAgent();
                return instance;
            }
        }

        public void Send(EmailEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("email entity can't be null");

            var config = EmailConfig.Instance;
            var client = new SmtpClient(config.SmtpServer)
            {
                Credentials = new NetworkCredential(config.SenderName, config.SenderPassword)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(config.SenderName),
                Subject = entity.Subject,
                Body = entity.Body
            };

            entity.To.ForEach(i => mail.To.Add(i + ";"));
            entity.Cc.ForEach(i => mail.CC.Add(i + ";"));

            client.Send(mail);
        }
    }
}
