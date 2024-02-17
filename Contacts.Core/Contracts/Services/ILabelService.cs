using Contacts.Core.Models;
using Contacts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Core.Contracts.Services;
public interface ILabelService
{
    Task<List<Label>> GetAllLabelsAsync();
    Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync();
    Task<Label> Upsert(Label label);
}
