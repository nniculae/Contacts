using Contacts.Behaviors;
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
        Loaded += ContactListPage_Loaded;
    }

    private void ContactListPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

        if (string.IsNullOrEmpty(ViewModel.InfoBarMessage))
            return;
        InfoCrud.Show(ViewModel.InfoBarMessage, 5000);
    }
}

