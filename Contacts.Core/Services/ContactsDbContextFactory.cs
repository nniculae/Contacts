using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Contacts.Core.Services;
public class ContactsDbContextFactory : IDesignTimeDbContextFactory<ContactsDbContext>
{
    public ContactsDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ContactsDbContext>();
        optionBuilder.EnableSensitiveDataLogging();
        optionBuilder.UseSqlite(Connection.GetSqliteConnection());
            //.LogTo(
            //    Console.WriteLine,
            //    new[] { DbLoggerCategory.Database.Command.Name },
            //    LogLevel.Information)
            //;

        return new ContactsDbContext(optionBuilder.Options);

    }
}
