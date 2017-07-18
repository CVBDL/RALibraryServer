using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaLibrary.Email;

namespace RALibrary.Tests
{
    [TestClass]
    public class RaLibraryEmailTest
    {
        [TestMethod]
        public void RalibraryEmailTest_Send_Success()
        {
            var entity = new EmailEntity();
            //entity.To.Add("test@***.com");
            //entity.Cc.Add("test@***.com");
            entity.Subject = EmailConfig.Instance.SubjectTemplate;
            entity.Body = EmailConfig.Instance.BodyTemplate;

            EmailAgent.Instance.Send(entity);
        }
    }
}
