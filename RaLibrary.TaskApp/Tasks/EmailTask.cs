using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;
using RaLibrary.Email;

namespace RaLibrary.TaskApp
{
    public class EmailTask : IScheduleTask
    {
        static ILog log = LogManager.GetLogger(typeof(EmailTask));

        public bool DoTask()
        {
            var task = GetExpiredBookData();
            return task.GetAwaiter().GetResult();
        }

        private async Task<bool> GetExpiredBookData()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("TODO");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<List<ExpiredBookInfo>>(data);
                    info.ForEach(i => SendMail(i));
                    return true;
                }
                else
                {
                    throw new Exception("Fail to get expired book information response.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return false;
        }

        private void SendMail(ExpiredBookInfo info)
        {
            // TODO: construct mail information.
            var entity = new EmailEntity();
            EmailAgent.Instance.Send(entity);
        }
    }
}
