using Bogus;
using Contacts.Core.Fakes;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Core.Services;
public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Label> Labels { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>()
       .HasMany(e => e.Labels)
       .WithMany(e => e.Contacts)
       .UsingEntity<ContactLabel>();

        modelBuilder.Entity<Contact>().Navigation(c => c.Address).AutoInclude();
        modelBuilder.Entity<Contact>().Navigation(c => c.Labels).AutoInclude();

        ContactGenerator.Init();
        modelBuilder.Entity<Contact>().HasData(ContactGenerator.Contacts);
        modelBuilder.Entity<Address>().HasData(ContactGenerator.Addresses);
        modelBuilder.Entity<Label>().HasData(ContactGenerator.Labels);
        modelBuilder.Entity<ContactLabel>().HasData(ContactGenerator.ContactLabels);
    }
}