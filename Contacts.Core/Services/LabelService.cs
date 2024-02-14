using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Core.Services;
public class LabelService(IDbContextFactory<ContactsDbContext> contextFactory) :ILabelService
{
    public async Task<List<Label>> GetAllLabelsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Labels.OrderBy(l => l.Name).AsNoTracking().ToListAsync();
    }

    public async Task<Label> Upsert(Label label)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        context.Update(label);

        await context.SaveChangesAsync();
        return label;
    }

}
