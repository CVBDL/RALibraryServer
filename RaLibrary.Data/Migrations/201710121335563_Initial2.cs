namespace RaLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Books", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Books", new[] { "Code" });
        }
    }
}
