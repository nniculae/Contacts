using Contacts.Core.Dto;
using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface ILabelService
{
    Task<List<Label>> GetAllLabelsAsync();
    Task<Label?> GetLabelByNameAsync(string name);
    Task<List<Label>> GetLabelsByContactIdAsync(int contactId);
    Task<List<Label>> GetAllOtherLabelsAsync(int contactId);
    Task<List<LabelsWithContactsCountDto>> GetLabelsWithContactsCountAsync();
    Task<List<Label>> GetNotAssociatedLabelsAsync();
    Task<Label> RemoveAsync(Label label);
    Task<Label> UpsertAsync(Label label);
}
