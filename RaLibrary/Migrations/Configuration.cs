namespace RaLibrary.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RaLibrary.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<RaLibrary.Models.RaLibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RaLibrary.Models.RaLibraryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Books.AddOrUpdate(x => x.Id,
                new Book()
                {
                    Id = 1,
                    CustomId = "P001",
                    Title = "Python Programming For Beginners"
                },
                new Book()
                {
                    Id = 2,
                    CustomId = "P002",
                    Title = "Java: The Ultimate Beginners Guide to Java Programming"
                },
                new Book()
                {
                    Id = 3,
                    CustomId = "P003",
                    Title = "C#: Become A Master In C#"
                }
            );
        }
    }
}
