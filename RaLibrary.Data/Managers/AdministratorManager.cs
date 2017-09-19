using RaLibrary.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class AdministratorManager
    {
        private RaLibraryContext _db = new RaLibraryContext();

        public bool AdministratorExists(string email)
        {
            return _db.Administrators.Count(admin => admin.Email == email) > 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
