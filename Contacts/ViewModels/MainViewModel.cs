using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;

namespace Contacts.ViewModels;

public partial class MainViewModel(IContactsService contactsService) : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private string _searchText;
    [ObservableProperty]
    private int _count;

    public ObservableGroupedCollection<string, Contact> ContactsDataSource
    {
        get;
        set;
    }


    public async void OnNavigatedTo(object parameter)
    {
        contacts = await contactsService.GetContactsAsync();

        var grouped = contacts.GroupBy(GetGroupName).OrderBy(g => g.Key);
        ContactsDataSource = new ObservableGroupedCollection<string, Contact>(grouped);
        Count = contacts.Count;




    }
    public void OnNavigatedFrom()
    {
        // do nothing
    }
    [RelayCommand]
    public void FilterTextChangedCommand()
    {
        /* Perform a Linq query to find all Contact objects (from the original Contact collection)
                that fit the criteria of the filter, save them in a new List called tempFiltered. */
        List<Contact> tempFiltered;
        tempFiltered = contacts.Where(contact => contact.ApplyFilter(SearchText)).ToList();
        RemoveContacts(tempFiltered);
        AddContacts(tempFiltered);
        Count = tempFiltered.Count;

    }
    private IList<Contact> contacts = [];
    private void AddContacts(List<Contact> tempFiltered)
    {

        foreach (Contact contact in tempFiltered)
        {
            string key = GetGroupName(contact);
            var group = ContactsDataSource.FirstGroupByKeyOrDefault(key);

            if (group != null && !group.Contains(contact))
            {
                ContactsDataSource.AddItem(key, contact);
            }
            else if (group == null)
            {
                ContactsDataSource.InsertItem(
                   key: key,
                   keyComparer: Comparer<string>.Default,
                   item: contact,
                   itemComparer: Comparer<Contact>.Create(
                       static (left, right) => Comparer<string>.Default.Compare(left.ToString(), right.ToString())));
            }
        }
    }


    /* Go through tempFiltered and compare it with the current ContactsSource collection,
            adding and subtracting items as necessary: */

    /// First, remove any Contact objects in ContactsSource that are not in tempFiltered
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
    private static string GetGroupName(Contact contact) => contact.Name.First().ToString().ToUpper();

}
