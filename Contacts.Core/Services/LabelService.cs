using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Core.Services;

public class LabelsWithContactsCountDto
{
    public Label Label { get; set; }
    public int ContactsCount { get; set; }
}

public class LabelService(IDbContextFactory<ContactsDbContext> contextFactory) : ILabelService
{
    public async Task<List<Label>> GetAllLabelsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        // is tracking
        return await context.Labels.OrderBy(l => l.Name).AsNoTracking().ToListAsync();
    }


    public async Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        return await context.Labels.Where(l => l.Contacts.Count > 0)
        .Select(l => new LabelsWithContactsCountDto { Label = l, ContactsCount = l.Contacts.Count })
        .AsNoTracking().ToListAsync();
    }

    public async Task<Label> Upsert(Label label)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        context.Update(label);

        await context.SaveChangesAsync();
        return label;
    }

}
