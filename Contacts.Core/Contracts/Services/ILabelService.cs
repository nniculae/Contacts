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
    Task<Label> Upsert(Label label);
}
