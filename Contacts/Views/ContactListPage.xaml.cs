using Contacts.Behaviors;
using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;

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
