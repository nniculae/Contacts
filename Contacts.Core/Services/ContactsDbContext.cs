using Contacts.Core.Fakes;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Contacts.Core.Services;
public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>().Navigation(c => c.Address).AutoInclude();

        ContactGenerator.Init();
        modelBuilder.Entity<Contact>().HasData(ContactGenerator.Contacts);
        modelBuilder.Entity<Address>().HasData(ContactGenerator.Addresses);
    }

    public override void Dispose()
    {
        Debug.WriteLine(nameof(Dispose) + " called");
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        Debug.WriteLine(nameof(DisposeAsync) + "called");
        return base.DisposeAsync();
    }


}