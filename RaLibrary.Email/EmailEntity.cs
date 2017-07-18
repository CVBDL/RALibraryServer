using System.Collections.Generic;

namespace RaLibrary.Email
{
    /// <summary>
    /// Provide the email input parameter.
    /// </summary>
    public class EmailEntity
    {
        public EmailEntity()
        {
            To = new List<string>();
            Cc = new List<string>();
        }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
