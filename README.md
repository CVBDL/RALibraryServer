# RA Library Server

## Required softwares

- Visual Studio 2013 Update 2 and above
- .Net Framework 4.5.1
- Microsoft ASP.NET Web API 2.2 5.2.3
- EntityFramework 6.1.3
- Microsoft SQL Server 2012

## FAQ

1. It shows *"Cannot attach the file *.mdf as database"* error.
    * Open SQL Server Object Explorer
    * Click refresh button.
    * Expand (localdb)\MSSQLLocalDB(SQL Server 12.x.xxxxx - xxxxx\xxxx)
    * Expand Database
    * Please remove existed same name database
    * Click right button and then delete
    * Go back to your Package Manage Console
    * Update-Database
