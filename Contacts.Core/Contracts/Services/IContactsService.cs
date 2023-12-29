using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface IContactsService
{
    Task AddContact(Contact contact);
    Task<List<Contact>> GetContactsAsync();
}