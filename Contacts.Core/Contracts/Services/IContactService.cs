using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface IContactService
{
    Task<Contact> Upsert(Contact contact);
    Task<List<Contact>> GetContactsAsync();
    Task<Contact> RemoveAsync(Contact contact);
    Task<Contact?> FindByIdAsync(int id);
    Task<List<Label>> GetLabelsAsync();
    Task<Contact> UpsertLabels(Contact contact);
    Task<List<Contact>> GetContactsByLabelIdAsync(int labelId);

    //public IList<IGrouping<string, Contact>> GetContactsGrouped();
}