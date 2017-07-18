using System.Xml.Linq;

namespace RaLibrary.Email
{
    /// <summary>
    /// Configuration of email.
    /// </summary>
    /// <remarks>
    /// Provide SMTP server and email template.
    /// </remarks>
    public class EmailConfig
    {
        public string SenderName { get; set; }

        public string SenderPassword { get; set; }

        public string SmtpServer { get; set; }

        public string SubjectTemplate { get; set; }

        public string BodyTemplate { get; set; }

        private EmailConfig()
        {
            Init();
        }

        private void Init()
        {
            var doc = XDocument.Load("MailParam.xml");
            SenderName = doc.Root.Element("senderName").Value;
            SenderPassword = doc.Root.Element("senderPassword").Value;
            SmtpServer = doc.Root.Element("smtpServer").Value;
            SubjectTemplate = doc.Root.Element("subjectTemplate").Value;
            BodyTemplate = doc.Root.Element("bodyTemplate").Value;
        }

        private static EmailConfig instance;
        public static EmailConfig Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmailConfig();
                return instance;
            }
        }
    }
}
