using Contacts.Core.Dto;
using Contacts.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Core.Contracts.Services;
public interface ILabelService
{
    Task<List<Label>> GetAllLabelsAsync();
    Task<List<Label>> GetLabelsByContactId(int contactId);
    Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync();
    Task<Label> RemoveAsync(Label label);
    Task<Label> Upsert(Label label);
}
