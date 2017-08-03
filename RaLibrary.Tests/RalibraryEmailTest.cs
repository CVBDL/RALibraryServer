using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaLibrary.Email;

namespace RALibrary.Tests
{
    [TestClass]
    public class RaLibraryEmailTest
    {
        [TestMethod]
        [Ignore]
        public void RalibraryEmailTest_Send_Success()
        {
            var entity = new EmailEntity();
            entity.Subject = EmailConfig.Instance.SubjectTemplate;
            entity.Body = string.Format(EmailConfig.Instance.BodyTemplate, "Colleague");
            EmailAgent.Instance.Send(entity);
        }
    }
}
