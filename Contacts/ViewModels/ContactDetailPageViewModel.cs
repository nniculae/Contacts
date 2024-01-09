using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;

namespace Contacts.ViewModels;

public partial class ContactDetailPageViewModel(IContactService contactsService, INavigationService navigation) :
    ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Contact _contact = null!;
    [ObservableProperty]
    private bool _isInEdit = false;
    [ObservableProperty]
    private bool _isNewContact = false;
    private Crud crud = Crud.Read;

    public void OnNavigatedFrom()
    {
        var message = new ContactChangedMessage(Contact, CrudStringMessage.FormatMessage(Contact.Name, crud));
        ((WeakReferenceMessenger)Messenger).Send(message);
    }

    public void StartEdit()
    {
        IsInEdit = true;
    }
    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Contact contact)
        {
            Contact = contact;
            IsNewContact = false;
            IsInEdit = false;
        }
        else
        {
            Contact = new Contact()
            {
                FirstName = string.Empty,
                Address = new Address()
            };

            IsNewContact = true;
            IsInEdit = true;
        }
    }

    public void GoBack()
    {
        navigation.GoBack();
    }

    [RelayCommand]
    public async Task Upsert()
    {
        await contactsService.Upsert(Contact);

        if (IsNewContact)
        {
            crud = Crud.Created;
        }
        else
        {
            crud = Crud.Updated;
        }

        GoBack();

    }

    [RelayCommand]
    public async Task Remove()
    {
        await contactsService.RemoveAsync(Contact);
        crud = Crud.Deleted;
        GoBack();
    }
}
