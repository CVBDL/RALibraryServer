using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace RAAuthentication
{
    public class Authentication
    {
        static public bool CheckAuthenticate(string userName, string password,string domainDame)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainDame, userName, password))
            {
                return pc.ValidateCredentials(userName, password, ContextOptions.Negotiate);
            }     
        }

        static public string GetUserEmailFromAD(string userName, string password, string domainName)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName, userName, password))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, userName);
                return user.EmailAddress;
            }
        }
    }
}
