using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Contacts.ViewModels;
//https://learn.microsoft.com/en-us/windows/uwp/enterprise/customer-database-tutorial
//https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
public partial class ContactsEditViewModel(IContactsService contactsService) : ObservableRecipient, INavigationAware
{

    [ObservableProperty]
    private Contact? _contact;

    public void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

    public void OnNavigatedTo(object parameter)
    {
        _contact = parameter as Contact;
    }

    [RelayCommand]
    public async Task Save()
    {
        if (Contact != null)
        {
            await contactsService.EditContact(Contact);
        }
    }
}
