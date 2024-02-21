using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Contacts.Core.Services;
public class ContactsDbContextFactory : IDesignTimeDbContextFactory<ContactsDbContext>
{
    public ContactsDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ContactsDbContext>();
        optionBuilder.EnableSensitiveDataLogging();
        optionBuilder.UseSqlite(Connection.GetSqliteConnection()).EnableSensitiveDataLogging();

        return new ContactsDbContext(optionBuilder.Options);
    }
}
