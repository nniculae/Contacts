using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Core.Contracts.Services;
using Contacts.Core.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Contacts.ViewModels;
//https://learn.microsoft.com/en-us/windows/uwp/enterprise/customer-database-tutorial
//https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
public partial class ContactDetailPageViewModel(IContactService contactsService, INavigationService navigation) : ObservableRecipient, INavigationAware
{

    [ObservableProperty]
    private Contact? _contact;
    [ObservableProperty]
    private bool _isInEdit = false;
    [ObservableProperty]
    private bool _isNewContact = false;

    private bool navigationModeHandled = false;

    public void OnNavigatedFrom()
    {
        //int i = 2;
        //navigation.Navigated += Navigation_Navigated;
       //navigation.Frame.Navigating -= Frame_Navigating;

    }

    private void Frame_Navigating(object sender, Microsoft.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
    {
        if (navigationModeHandled) return;


        void resumeNavigation()
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
                // navigation.GoBack();
                
                navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, Contact, true);
                
            }
            else
            {
               navigation.Frame.Navigate(e.SourcePageType, e.Parameter, e.NavigationTransitionInfo);
            }
            navigationModeHandled = true; ;
        }

        resumeNavigation();

    }
   

    public void OnNavigatedTo(object parameter)
    {
       // navigation.Frame.Navigating += Frame_Navigating;

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

    public void StartEdit()
    {
        IsInEdit = true;

    }
    public void CancelEditsAsync() {
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, Contact, true);
    }

    [RelayCommand]
    public async Task Upsert()
    {
        if (Contact != null)
        {
            await contactsService.Upsert(Contact);
            navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, Contact, true);
        }
    }

    [RelayCommand]
    public async Task Remove()
    {
        if(Contact == null)
            return;
        await contactsService.RemoveAsync(Contact);
        navigation.GoBack();
    }
}
