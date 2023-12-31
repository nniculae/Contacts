using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface IContactService
{
    Task<Contact> Upsert(Contact contact);
    Task<IList<Contact>> GetContactsAsync();
    public IList<IGrouping<string, Contact>> GetContactsGrouped();
    Task<Contact> RemoveAsync(Contact contact);
}