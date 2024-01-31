using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;

namespace Contacts.ViewModels;

public partial class ContactDetailPageViewModel(
    IContactService contactsService,
    INavigationService navigation) :
    ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Contact? _contact;
    [ObservableProperty]
    private bool _isInEdit = false;
    [ObservableProperty]
    private bool _isNewContact = false;
    private Crud crud = Crud.Read;

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is int id)
        {
            Contact = await contactsService.FindByIdAsync(id);
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
    public void OnNavigatedFrom()
    {
        var message = new ContactChangedMessage(Contact!.Id, CrudStringMessage.FormatMessage(Contact.Name, crud));
        ((WeakReferenceMessenger)Messenger).Send(message);
    }
    public void GoBack()
    {
        navigation.GoBack();
    }
    public void StartEdit()
    {
        IsInEdit = true;
    }
    [RelayCommand]
    public async Task UpsertAsync()
    {
        await contactsService.Upsert(Contact!);

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
    public async Task RemoveAsync()
    {
        await contactsService.RemoveAsync(Contact!);
        crud = Crud.Deleted;
        GoBack();
    }
}
