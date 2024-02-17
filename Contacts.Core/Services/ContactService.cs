﻿using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Contacts.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DesignPatternsUI.Core.Services;


public class ContactService(IDbContextFactory<ContactsDbContext> contextFactory) : IContactService
{
    public async Task<List<Contact>> GetContactsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Contacts.OrderBy(c => c.FirstName).AsNoTracking().ToListAsync();
    }

    public async Task<List<Contact>> GetContactsByLabelIdAsync(int labelId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Contacts.Where(c => c.Labels.FirstOrDefault(l => l.Id == labelId) != null).OrderBy(c => c.FirstName).AsNoTracking().ToListAsync();
    }

    public async Task<List<Label>> GetLabelsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Labels.OrderBy(l => l.Name).AsNoTracking().ToListAsync();
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

    public async Task<Contact> UpsertLabels(Contact contact)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var dbContact = await context.Contacts.FindAsync(contact.Id);

        var existingLabelIds = dbContact.Labels.Select(l => l.Id);
        var newLabelIds = contact.Labels.Select(l => l.Id);

        var labelsToRemove = dbContact.Labels.Where(l => !newLabelIds.Contains(l.Id)).ToList();
        var labelsToAdd = contact.Labels.Where(l => !existingLabelIds.Contains(l.Id)).ToList();

        foreach (var label in labelsToRemove)
        {
            dbContact.Labels.Remove(label);
        }

        foreach (var label in labelsToAdd)
        {
            dbContact.Labels.Add(label);
        }

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
}
