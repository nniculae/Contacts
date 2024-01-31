using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DesignPatternsUI.Core.Services;


public class ContactsService(IDbContextFactory<ContactsDbContext> contextFactory) : IContactService
{

    public async Task<List<Contact>> GetContactsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Contacts.OrderBy(c => c.FirstName).AsNoTracking().ToListAsync();
    }

    public async Task<Contact?> FindByIdAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Contacts.FindAsync(id);
    }

    public async Task<Contact> Upsert(Contact contact)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        context.Update(contact);
        await context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> RemoveAsync(Contact contact)
    {
        await using var context = contextFactory.CreateDbContext();
        context.Update(contact);
        context.Remove(contact);
        await context.SaveChangesAsync();
        return contact;
    }

    //public string GetGroupName(Contact contact) => contact.Name.First().ToString().ToUpper();

    //public IList<IGrouping<string, Contact>> GetContactsGrouped()
    //{
    //    using var context = contextFactory.CreateDbContext();
    //    return context.Contacts.GroupBy(GetGroupName).OrderBy(g => g.Key).ToList();
    //}
}
