using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Extensions;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Contacts.ViewModels;

public partial class ContactListPageViewModel(IContactService contactsService, INavigationService navigation)
    : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private string? _searchText;
    [ObservableProperty]
    private int _count;
    [ObservableProperty]
    private Contact? _selectedItem;
    [ObservableProperty]
    public string _infoBarMessage = string.Empty;

    private bool _isBackFromDetails = false;
    private IList<Contact> _contacts = [];


    public ObservableGroupedCollection<string, Contact> ContactsDataSource { get; set; } = [];


    public async void OnNavigatedTo(object parameter)
    {
        _contacts = await contactsService.GetContactsAsync();
        var grouped = _contacts.GroupBy(CreateKey).OrderBy(g => g.Key);
        ContactsDataSource = new ObservableGroupedCollection<string, Contact>(grouped);
        Count = ContactsDataSource.CountItems();

        if (parameter is ContactParameterWrapper contactParameterWrapper)
        {

            Contact? refreshedContact = ContactsDataSource.FindItem(contactParameterWrapper.Contact);
            if (refreshedContact != null)
            {
                SelectedItem = refreshedContact;
            }

            _isBackFromDetails = true;
            // maybe raise an event, message mvvm 
            InfoBarMessage = contactParameterWrapper.InfoBarMessage;

        }
        else
        {
            EnsureItemSelected();
        }

    }
    public void OnNavigatedFrom()
    {
        // do nothing
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

    public void EnsureItemSelected() => SelectedItem ??= _contacts.FirstOrDefault();

    public void ContactListView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

        if (_isBackFromDetails)
        {
            var listView = (ListView)sender;
            listView.ScrollIntoView(listView.SelectedItem, ScrollIntoViewAlignment.Leading);
        }

    }

}
