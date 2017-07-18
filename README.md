# RA Library Server

## FAQ

- It shows "Cannot attach the file *.mdf as database" error.

  1. Open SQL Server Object Explorer
  1. Click refresh button.
  1. Expand (localdb)\MSSQLLocalDB(SQL Server 12.x.xxxxx - xxxxx\xxxx)
  1. Expand Database
  1. Please remove existed same name database
  1. Click right button and then delete
  1. Go back to your Package Manage Console
  1. Update-Database
