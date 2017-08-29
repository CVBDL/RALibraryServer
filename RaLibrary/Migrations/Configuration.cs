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
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
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
                    Code = "P001",
                    Title = "Python Programming For Beginners"
                },
                new Book()
                {
                    Id = 2,
                    Code = "P002",
                    Title = "Java: The Ultimate Beginners Guide to Java Programming"
                },
                new Book()
                {
                    Id = 3,
                    Code = "P003",
                    Title = "C#: Become A Master In C#"
                },
                new Book()
                {
                    Id = 4,
                    Code = "A-001",
                    Title = "Advanced FrameMaker"
                },
                new Book()
                {
                    Id = 5,
                    Code = "A003",
                    Title = "代码大全第二版"
                },
                new Book()
                {
                    Id = 6,
                    Code = "A005",
                    Title = "J2ME 开发精解"
                },
                new Book()
                {
                    Id = 7,
                    Code = "A006",
                    Title = "J2ME 无线平台应用开发"
                },
                new Book()
                {
                    Id = 8,
                    Code = "A011",
                    Title = "Java 夜未眠"
                },
                new Book()
                {
                    Id = 9,
                    Code = "G001",
                    Title = "A Guide to the Project Management Body of Knowledg"
                },
                new Book()
                {
                    Id = 10,
                    Code = "G002",
                    Title = "现代卓越PMP考试"
                }
            );
        }
    }
}
