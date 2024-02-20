using Contacts.Core.Contracts.Services;
using Contacts.Core.Dto;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Core.Services;

public class LabelService(IDbContextFactory<ContactsDbContext> contextFactory) : ILabelService
{
    public async Task<List<Label>> GetAllLabelsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context
            .Labels
            .OrderBy(l => l.Name)
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Labels.Where(l => l.Contacts.Count > 0)
            .Select(l => new LabelsWithContactsCountDto { Label = l, ContactsCount = l.Contacts.Count })
            .OrderBy(l => l.Label.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Label>> GetNotAssociatedLabels()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Labels.Where(l => l.Contacts.Count == 0)
            .OrderBy(l => l.Name)
            .Select(l => l)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Label> Upsert(Label label)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        context.Update(label);

        await context.SaveChangesAsync();
        return label;
    }

    public async Task<Label> RemoveAsync(Label label)
    {
        await using var context = contextFactory.CreateDbContext();
        context.Update(label);
        context.Remove(label);
        await context.SaveChangesAsync();
        return label;
    }

    public async Task<List<Label>> GetLabelsByContactId(int contactId)
    {
        await using var context = contextFactory.CreateDbContext();
       
        return await context
            .Labels
            .Where(l => l.Contacts.FirstOrDefault(c => c.Id == contactId) != null)
            .OrderBy(l => l.Name)
            .AsNoTracking()
            .ToListAsync();
            
    }

}
