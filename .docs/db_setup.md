# Database setup guide

## Initial setup
Use this instruction if it's initial setup and you have not created database earlier.

1. Create MDF file and move it to propper location. <br>
You can create MDF file in VisualStudio by adding new ***Service-based database*** to DB project. <br>
To do so rightclick on DB project -> Add -> New Item -> ***Service-based database*** and name it ***db.mdf***. <br>
When DB is created open it's location or navigate to `ProjektMiASI\Backend\DB` and cut files ***db.mdf*** and ***db_log.ldf***. <br>
Navigate to `ProjektMiASI\Backend\.database` (if this directory does not exist create it) and paste files there. <br>
2. Apply migrations to the Database. <br>
Open termianl in `ProjektMiASI\Backend\DB`. <br>
Install dotnet ef tools by executing this command: `dotnet tool install --global dotnet-ef`. <br>
Apply migrations to the db by executing this command: `dotnet ef database update`. <br>
3. Test the databse setup by launching API project and register new user.

## Updating database to new migrations
Use this instruction if you already had database setup and want to update it to new migrations.

Apply migrations to the db by executing this command: `dotnet ef database update`.

## Connecting to local db with Microsoft SQL Server Management Studio.
This is optional and only necessary if you want to use mentioned tool.

1. Download and install the tool from [Microsoft site](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16).
2. Open the tool and set the fields as follows:
* Server type: Database Engine
* Server Name: `(LocalDB)\MSSQLLocalDB`
* Authentication: Windows Authentication
3. After connectng unfold Databases and you should see database called db.
