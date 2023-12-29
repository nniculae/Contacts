using Contacts.Core.Fakes;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Core.Services;
public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options) { }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ContactGenerator.Init();
        _ = modelBuilder.Entity<Contact>().HasData(ContactGenerator.Contacts);
        _ = modelBuilder.Entity<Address>().HasData(ContactGenerator.Addresses);
    }
}