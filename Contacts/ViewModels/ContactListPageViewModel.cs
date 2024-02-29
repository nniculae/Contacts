using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Animation;

namespace Contacts.ViewModels;

public partial class ContactListPageViewModel(IContactService contactsService, INavigationService navigation)
    : ObservableRecipient, INavigationAware, IRecipient<ContactChangedMessage>
{
    [ObservableProperty]
    private string _searchText = string.Empty;
    [ObservableProperty]
    private int _count;
    [ObservableProperty]
    private Contact? _selectedItem;
    [ObservableProperty]
    private Label _selectedLabel = null!;
    public ObservableGroupedCollection<string, Contact> ContactsDataSource { get; set; } = [];
    public string InfoBarMessage { get; set; } = string.Empty;
    public bool IsBackFromDetails { get; set; } = false;
    private IList<Contact> _contacts = [];
    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public async Task OnNavigatedTo(object parameter)
    {

        await dispatcherQueue.EnqueueCustomAsync(() =>
        {
            IsActive = true;
            ContactsDataSource.Clear();
        });

        if (parameter is int labelid)
        {
            _contacts = await contactsService.GetContactsByLabelIdAsync(labelid);
        }
        else
        {
            _contacts = await contactsService.GetContactsAsync();
        }

        

        await dispatcherQueue.EnqueueCustomAsync(() =>
        {
            var groupings = _contacts.GroupBy(CreateKey).OrderBy(g => g.Key);
            foreach (var item in groupings)
            {
                ContactsDataSource.AddGroup(item);
            }

            Count = ContactsDataSource.CountItems();
            EnsureItemSelected();
        });
    }

    public async Task OnNavigatedFrom()
    {
        await dispatcherQueue.EnqueueCustomAsync(() =>
        {
            contactsService.Dispose();
            IsActive = false;
        });
        
    }

    [RelayCommand]
    public void NavigateToContactDetailPage()
    {
        if (SelectedItem != null)
        {
            int id = SelectedItem.Id;
            navigation.NavigateToWithAnimation(typeof(ContactDetailPageViewModel).FullName!,
               id, false, new DrillInNavigationTransitionInfo());
        }
    }
    [RelayCommand]
    public void NavigateToCreate()
    {
        navigation.NavigateTo(typeof(ContactDetailPageViewModel).FullName!, null);
    }

    partial void OnSelectedLabelChanged(Label value)
    {
        // nu e bine stop bubbling, use an event, studiza problema
        // nici linkurile nu-s bune, se vede tooltip, 
        if (value == null) return;
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, value.Id);

    }
    async partial void OnSearchTextChanged(string value)
    {
        await dispatcherQueue.EnqueueCustomAsync(() =>
        {
            IEnumerable<Contact> tempFiltered = _contacts.Where(contact => contact.ApplyFilter(value));

            var keyComparer = Comparer<string>.Default;
            var itemComparer = Comparer<Contact>.Create((left, right) =>
                        keyComparer.Compare(left.Name, right.Name));

            ContactsDataSource.FilterItems(tempFiltered, keyComparer, itemComparer, CreateKey);
            Count = ContactsDataSource.CountItems();
        });

    }

    private static string CreateKey(Contact contact)
    {
        return contact.Name[0].ToString().ToUpperInvariant();
    }

    private void EnsureItemSelected() => SelectedItem ??= _contacts.FirstOrDefault();

    public async void Receive(ContactChangedMessage message)
    {
        await dispatcherQueue.EnqueueCustomAsync(() =>
        {
            InfoBarMessage = message.StringMessage;

            int contactId = message.Value;

            if (contactId > 0)
            {
                var ogcAsEnumerable = (IEnumerable<ObservableGroup<string, Contact>>)ContactsDataSource;
                Contact? refreshedContact = ogcAsEnumerable.SelectMany(group => group)
                          .FirstOrDefault(contact => contact.Id == contactId);
                SelectedItem = refreshedContact;
            }
            else
            {
                EnsureItemSelected();
            }

            IsBackFromDetails = true;
        });
    }

}
