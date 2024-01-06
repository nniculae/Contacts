using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Contacts.ViewModels;

public partial class ContactListPageViewModel(IContactService contactsService, INavigationService navigation) : ObservableRecipient, INavigationAware
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
        var grouped = _contacts.GroupBy(GetGroupName).OrderBy(g => g.Key);
        ContactsDataSource = new ObservableGroupedCollection<string, Contact>(grouped);
        Count = _contacts.Count;

        if (parameter is ContactParameterWrapper contactParameterWrapper)
        {
            Contact? refreshedContact = FindContact(contactParameterWrapper.Contact.Id);
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
        /* Perform a Linq query to find all Contact objects (from the original Contact collection)
                that fit the criteria of the filter, save them in a new List called tempFiltered. */

        List<Contact> tempFiltered = _contacts.Where(contact => contact.ApplyFilter(SearchText)).ToList();
        RemoveContacts(tempFiltered);
        AddContacts(tempFiltered);
        Count = tempFiltered.Count;

    }

    //TODO: It's not sorted correctly because some items were allready in the ContactsDataSource

    /* Next, add back any Person objects that are included in tempFiltered and may 
   not currently be in ContactsDataSource (in case of a backspace) */
    //private void AddContacts(List<Contact> tempFiltered)
    //{

    //    foreach (Contact contact in tempFiltered)
    //    {
    //        string key = GetGroupName(contact);
    //        ObservableGroup<string, Contact>? group = ContactsDataSource.FirstGroupByKeyOrDefault(key);

    //        if (group != null && !group.Contains(contact))
    //        {
    //            ContactsDataSource.AddItem(key, contact);
    //        }
    //        else if (group == null)
    //        {
    //            ContactsDataSource.InsertItem(
    //               key: key,
    //               keyComparer: Comparer<string>.Default,
    //               item: contact,
    //               itemComparer: Comparer<Contact>.Create(
    //                   static (left, right) => Comparer<string>.Default.Compare(left.Name, right.Name)));
    //        }
    //    }
    //}

    /* Next, add back any Contact objects that are included in tempFiltered and may 
  not currently be in ContactsDataSource (in case of a backspace) */
    private void AddContacts(List<Contact> tempFiltered)
    {

        foreach (Contact contact in tempFiltered)
        {

            if (FindContact(contact.Id) is not null)
            {
                continue;
            }
            else { 

            ContactsDataSource.InsertItem(
               key: GetGroupName(contact),
               keyComparer: Comparer<string>.Default,
               item: contact,
               itemComparer: Comparer<Contact>.Create(
                   static (left, right) => Comparer<string>.Default.Compare(left.Name, right.Name)));
            }

        }
    }


    /* Go through tempFiltered and compare it with the current ContactsSource collection,
            adding and subtracting items as necessary: */

    /// First, remove any Contact objects in ContactsDataSource that are not in tempFiltered
    private void RemoveContacts(List<Contact> tempFiltered)
    {
        for (int i = ContactsDataSource.Count - 1; i >= 0; i--)
        {
            ObservableGroup<string, Contact> observableGroup = ContactsDataSource[i];
            foreach (Contact contact in observableGroup.Reverse())
            {
                if (!tempFiltered.Contains(contact))
                {
                    observableGroup.Remove(contact);

                }
            }
            if (observableGroup.Count == 0)
            {
                ContactsDataSource.Remove(observableGroup);
            }
        }
    }

    public Contact? FindContact(int id)
    {
        foreach (ObservableGroup<string, Contact> observableGroup in ContactsDataSource)
        {
            foreach (Contact contact in observableGroup)
            {
                if (contact.Id == id)
                {
                    return contact;
                }
            }

        }

        return null;
    }

    private static string GetGroupName(Contact contact) => contact.Name.First().ToString().ToUpper();
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
