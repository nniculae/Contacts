using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DesignPatternsUI.Core.Services;

public class ContactService : IContactService
{
    private readonly ContactsDbContext _context;
    private readonly IDbContextFactory<ContactsDbContext> _contextFactory;

    public ContactService(IDbContextFactory<ContactsDbContext> contextFactory)
    {

        _context = contextFactory.CreateDbContext();
        _contextFactory = contextFactory;
    }

    public async Task<List<Contact>> GetContactsAsync()
    {

        //await using var context = await contextFactory.CreateDbContextAsync();
        return await _context
            .Contacts
            .OrderBy(c => c.FirstName)
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Contact>> GetContactsByLabelIdAsync(int labelId)
    {
        //await using var context = await contextFactory.CreateDbContextAsync();
        return await _context
            .Contacts
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .Where(c => c.Labels.FirstOrDefault(l => l.Id == labelId) != null)
            .OrderBy(c => c.FirstName)
            .AsNoTracking()
            .ToListAsync();
    }

    //public async Task<List<Label>> GetLabelsAsync()
    //{
    //    //await using var context = await contextFactory.CreateDbContextAsync();
    //    return await context
    //        .Labels
    //        .OrderBy(l => l.Name)
    //        .AsNoTracking()
    //        .ToListAsync();
    //}
    public async Task<Contact> FindByIdAsync(int id)
    {
        //await using var context = await contextFactory.CreateDbContextAsync();
        return await _context
            .Contacts
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .Include(c => c.Address)
            .Include(c => c.ContactLabels)
            //.AsNoTracking()
            .FirstAsync(c => c.Id == id);
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        //await using var context = await contextFactory.CreateDbContextAsync();

        _context.Update(contact);

        await _context.SaveChangesAsync();
        return contact;
    }
    // https://www.thereformedprogrammer.net/updating-many-to-many-relationships-in-entity-framework-core/

    //public async Task<Contact> UpsertLabels(Contact contact, bool isNew = false)
    //{
    //    //TODO: It's too complex, find another solution 
    //    //await using var context = await contextFactory.CreateDbContextAsync();

    //    if (isNew)
    //    {
    //        context.Update(contact);
    //        await context.SaveChangesAsync();
    //        return contact;
    //    }


    //    var dbContact = await context.Contacts.Include(c => c.Labels).FirstAsync(c => c.Id == contact.Id);

    //    var existingLabelIds = dbContact.Labels.Select(l => l.Id);
    //    var newLabelIds = contact.Labels.Select(l => l.Id);

    //    var labelsToRemove = dbContact.Labels.Where(l => !newLabelIds.Contains(l.Id)).ToList();
    //    var labelsToAdd = contact.Labels.Where(l => !existingLabelIds.Contains(l.Id)).ToList();


    //    foreach (var label in labelsToRemove)
    //    {
    //        dbContact.Labels.Remove(label);
    //    }


    //    foreach (var label in labelsToAdd)
    //    {
    //        dbContact.Labels.Add(label);
    //    }

    //    await context.SaveChangesAsync();
    //    return contact;
    //}

    //public async Task<Contact> UpsertLabels(Contact contact, bool isNew = false)
    //{
    //    //TODO: It's too complex, find another solution 
    //    //await using var context = await contextFactory.CreateDbContextAsync();

    //    if (isNew)
    //    {
    //        context.Update(contact);
    //        await context.SaveChangesAsync();
    //        return contact;
    //    }


    //    var dbContact = await context.Contacts.Include(c => c.Labels).FirstAsync(c => c.Id == contact.Id);

    //    var existingLabelIds = dbContact.Labels.Select(l => l.Id);
    //    var newLabelIds = contact.Labels.Select(l => l.Id);

    //    var labelsToRemove = dbContact.Labels.Where(l => !newLabelIds.Contains(l.Id)).ToList();
    //    var labelsToAdd = contact.Labels.Where(l => !existingLabelIds.Contains(l.Id)).ToList();


    //    foreach (var label in labelsToRemove)
    //    {
    //        dbContact.Labels.Remove(label);
    //    }


    //    foreach (var label in labelsToAdd)
    //    {
    //        dbContact.Labels.Add(label);
    //    }

    //    await context.SaveChangesAsync();
    //    return contact;
    //}


    public async Task<int> SaveAsync()
    {
        _context.ChangeTracker.DetectChanges();
        var dbg = _context.ChangeTracker.DebugView.LongView;
        return await _context.SaveChangesAsync();
    }

    public async Task<Contact> RemoveAsync(Contact contact)
    {
        //await using var context = contextFactory.CreateDbContext();
        _context.Update(contact);
        _context.Remove(contact);
        await _context.SaveChangesAsync();
        return contact;
    }
    #region IDisposable Implementation
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
    public void Dispose()
    {
        Debug.WriteLine("Context disposed");

        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion 
}
