using RaLibrary.Data.Context;
using System.Linq;

namespace RaLibrary.Data.Managers
{
    public class AdministratorManager
    {
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        public bool IsAdministrator(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return _db.Administrators.Count(admin => admin.Email == email) > 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
