using Contacts.Behaviors;
using Contacts.Core.Models;
using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices.ObjectiveC;
using Windows.ApplicationModel.Contacts;

namespace Contacts.Views;

public sealed partial class ContactListPage : Page
{
    public ContactListPageViewModel ViewModel
    {
        get;
    }

    public ContactListPage()
    {
        ViewModel = App.GetService<ContactListPageViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderMode(this, NavigationViewHeaderMode.Never);
    }
     
}
