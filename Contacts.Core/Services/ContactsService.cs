using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DesignPatternsUI.Core.Services;


public class ContactsService(IDbContextFactory<ContactsDbContext> contextFactory) : IContactsService
{

    public async Task<IList<Contact>> GetContactsAsync()
    {
        using var context = contextFactory.CreateDbContext();
        return await context.Contacts.ToListAsync();    
    }


    public IList<IGrouping<string, Contact>> GetContactsGrouped()
    {
        using var context = contextFactory.CreateDbContext();
        return  context.Contacts.GroupBy(GetGroupName).OrderBy(g => g.Key).ToList();
    }


    public async Task<IList<Contact>> GetContactsFilteredAsync(string filter)
    {
        using var context = contextFactory.CreateDbContext();
        return await context.Contacts.Where(contact => contact.ApplyFilter(filter)).ToListAsync();
    }

    public async Task AddContact(Contact contact)
    {
        using var context = contextFactory.CreateDbContext();
        _ = await context.AddAsync(contact);
        _ = await context.SaveChangesAsync();
    }

    public string GetGroupName(Contact contact) => contact.Name.First().ToString().ToUpper();
}
