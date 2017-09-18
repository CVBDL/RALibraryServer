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
        private RaLibraryContext db = new RaLibraryContext();

        public bool AdministratorExists(string email)
        {
            return db.Administrators.Count(admin => admin.Email == email) > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
