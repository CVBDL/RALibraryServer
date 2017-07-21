using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAAuthentication
{
    public class UserInfo
    {
        public UserInfo(string name, string email)
        {
            userName = name;
            emailAddress = email;
        }
        public string userName { get; set; }
        public string emailAddress { get; set; }
    }
}
