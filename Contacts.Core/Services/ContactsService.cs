using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DesignPatternsUI.Core.Services;
public class ContactsService(IDbContextFactory<ContactsDbContext> contextFactory) : IContactsService
{

    public async Task<List<Contact>> GetContactsAsync()
    {
        using var context = contextFactory.CreateDbContext();
        return await context.Contacts.ToListAsync();    
    }

    public async Task AddContact(Contact contact)
    {
        using var context = contextFactory.CreateDbContext();
        _ = await context.AddAsync(contact);
        _ = await context.SaveChangesAsync();
    }
}
