﻿using Contacts.Core.Models;

namespace Contacts.Core.Contracts.Services;
public interface IContactService : IDisposable
{
    Task<Contact> AddAsync(Contact contact);
    Task<List<Contact>> GetContactsAsync();
    Task<Contact> RemoveAsync(Contact contact);
    Task<Contact> FindByIdAsync(int id);
    Task<List<Contact>> GetContactsByLabelIdAsync(int labelId);
    Task<int> SaveAsync();
}