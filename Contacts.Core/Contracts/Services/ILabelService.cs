using Contacts.Core.Dto;
using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface ILabelService
{
    Task<List<Label>> GetAllLabelsAsync();
    Task<List<Label>> GetLabelsByContactId(int contactId);
    Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync();
    Task<List<Label>> GetNotAssociatedLabels();
    Task<Label> RemoveAsync(Label label);
    Task<Label> UpsertAsync(Label label);
}
