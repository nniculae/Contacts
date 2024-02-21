using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DesignPatternsUI.Core.Services;

public class ContactService(IDbContextFactory<ContactsDbContext> contextFactory) : IContactService
{
    private readonly ContactsDbContext _context = contextFactory.CreateDbContext();

    public async Task<List<Contact>> GetContactsAsync()
    {
        return await _context
            .Contacts
            .OrderBy(c => c.FirstName)
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Contact>> GetContactsByLabelIdAsync(int labelId)
    {
        return await _context
            .Contacts
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .Where(c => c.Labels.FirstOrDefault(l => l.Id == labelId) != null)
            .OrderBy(c => c.FirstName)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contact> FindByIdAsync(int id)
    {

        return await _context
            .Contacts
            .Include(c => c.Labels.OrderBy(l => l.Name))
            .Include(c => c.Address)
            .Include(c => c.ContactLabels)
            .FirstAsync(c => c.Id == id);
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        _context.Update(contact);

        await _context.SaveChangesAsync();
        return contact;
    }
    public async Task<int> SaveAsync()
    {
        //_context.ChangeTracker.DetectChanges();
        //var dbg = _context.ChangeTracker.DebugView.LongView;
        return await _context.SaveChangesAsync();
    }

    public async Task<Contact> RemoveAsync(Contact contact)
    {
        _context.Update(contact);
        _context.Remove(contact);
        await _context.SaveChangesAsync();
        return contact;
    }
    #region IDisposable Implementation
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion 
}
