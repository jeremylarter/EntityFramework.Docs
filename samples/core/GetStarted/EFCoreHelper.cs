using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFGetStarted
{
    public class EFCoreHelper
    {
        public static Dictionary<string, string> GetAllMigrationsSQL(DbContext db)
        {            
            var migrationsSQL = new Dictionary<string, string>();
            var migrations = db.Database.GetMigrations();
            var migrator = db.GetService<IMigrator>();
            string fromMigration = null;
            foreach(var toMigration in migrations)
            {
                var sql = migrator.GenerateScript(fromMigration, toMigration);
                migrationsSQL.Add(toMigration, sql);
                fromMigration = toMigration;
            }
            return migrationsSQL;
        }

        public static void LogMigrations(Dictionary<string, string> migrations, Action<string> writeLine)
        {
            foreach(var migration in migrations)
            {
                writeLine(migration.Key);
                writeLine(migration.Value);
            }
        }

        public static void LogMigrations(DbContext db, Action<string> writeLine = null)
        {
            if (writeLine == null) writeLine = Console.WriteLine;
            var migrations = EFCoreHelper.GetAllMigrationsSQL(db);
            EFCoreHelper.LogMigrations(migrations, writeLine);
        }

        public static string DbUpdateExceptionInstructions => "Exception: An error occurred while updating the entries."
                    + "\r\n\r\nThis usually happens when the database has not been created and/or the migrations have not"
                    + "\r\nbeen applied to the database."
                    + "\r\nThe usual way to fix this is to run the .NET Core CLI commands: "
                    + "\r\nSee: https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli#create-the-database"
                    + "\r\n\tdotnet tool install --global dotnet-ef"
                    + "\r\n\tdotnet add package Microsoft.EntityFrameworkCore.Design"
                    + "\r\n\tdotnet ef migrations add InitialCreate"
                    + "\r\n\tdotnet ef database update"
                    + "\r\n\r\nFor this example, we just want to get the SQL for the migration to apply independently.";
    }
}

