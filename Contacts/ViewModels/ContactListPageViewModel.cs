using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Extensions;
using Microsoft.UI.Xaml.Media.Animation;

namespace Contacts.ViewModels;

public partial class ContactListPageViewModel(IContactService contactsService, INavigationService navigation)
    : ObservableRecipient, INavigationAware, IRecipient<ContactChangedMessage>
{
    [ObservableProperty]
    private string? _searchText;
    [ObservableProperty]
    private int _count;
    [ObservableProperty]
    private Contact? _selectedItem;
    public ObservableGroupedCollection<string, Contact> ContactsDataSource { get; set; } = [];

    public string InfoBarMessage { get; set; } = string.Empty;
    public bool IsBackFromDetails { get; set; } = false;

    private IList<Contact> _contacts = [];

    public async void OnNavigatedTo(object parameter)
    {
        IsActive = true;
        _contacts = await contactsService.GetContactsAsync();
        var grouped = _contacts.GroupBy(CreateKey).OrderBy(g => g.Key);
        ContactsDataSource = new ObservableGroupedCollection<string, Contact>(grouped);
        Count = ContactsDataSource.CountItems();
        EnsureItemSelected();

    }
    public void OnNavigatedFrom()
    {
        IsActive = false;
    }
    [RelayCommand]
    public void NavigateToContactDetailPage()
    {
        if (SelectedItem != null)
        {
            navigation.NavigateToWithAnimation(typeof(ContactDetailPageViewModel).FullName!,
                SelectedItem, false, new DrillInNavigationTransitionInfo());
        }
    }
    [RelayCommand]
    public void NavigateToCreate()
    {
        navigation.NavigateTo(typeof(ContactDetailPageViewModel).FullName!, null);
    }
    [RelayCommand]
    public void FilterTextChangedCommand()
    {


        IEnumerable<Contact> tempFiltered = _contacts.Where(contact => contact.ApplyFilter(SearchText));

        var keyComparer = Comparer<string>.Default;
        var itemComparer = Comparer<Contact>.Create((left, right) =>
                    keyComparer.Compare(left.Name, right.Name));

        ContactsDataSource.FilterItems(tempFiltered, keyComparer, itemComparer, CreateKey);
        Count = ContactsDataSource.CountItems();

    }
    private static string CreateKey(Contact contact)
    {
        return contact.Name.First().ToString().ToUpperInvariant();
    }
    private void EnsureItemSelected() => SelectedItem ??= _contacts.FirstOrDefault();
    public void Receive(ContactChangedMessage message)
    {
        InfoBarMessage = message.StringMessage;

        Contact contact = message.Value;
        if (message.Value != null)
        {
            Contact? refreshedContact = ContactsDataSource.FindItem(contact);
            if (refreshedContact != null)
            {
                SelectedItem = refreshedContact;
            }

            IsBackFromDetails = true;
        }
        else
        {
            EnsureItemSelected();
        }
    }
}
