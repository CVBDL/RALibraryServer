using RaLibrary.Data.Entities;
using System.Data.Entity;

namespace RaLibrary.Data.Context
{
    internal class RaLibraryContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public RaLibraryContext() : base("name=RaLibraryContext") { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowLog> BorrowLogs { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
    }
}
