using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface IContactsService
{
    Task AddContact(Contact contact);
    Task<int> EditContact(Contact contact);
    Task<IList<Contact>> GetContactsAsync();
    public IList<IGrouping<string, Contact>> GetContactsGrouped();
}