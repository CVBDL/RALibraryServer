using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class ServiceAccountManager
    {
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        public async Task<bool> IsValidServiceAccount(string username, string password)
        {
            string md5Passowrd = GetMd5Hash(password);

            var account = await _db.ServiceAccounts
                .Where(sa => sa.Username == username
                             && sa.Password == md5Passowrd)
                .FirstOrDefaultAsync();

            return (account != null);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private string GetMd5Hash(string source)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
