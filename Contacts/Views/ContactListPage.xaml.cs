using Contacts.Behaviors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Contacts.Views;
public sealed partial class ContactListPage : Page
{
    public ContactListPageViewModel ViewModel { get; }
    public ContactListPage()
    {
        ViewModel = App.GetService<ContactListPageViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderMode(this, NavigationViewHeaderMode.Never);
        Loaded += ContactListPage_Loaded;
    }
    private void ContactListPage_Loaded(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(ViewModel.InfoBarMessage))
            return;
        InfoCrud.Show(ViewModel.InfoBarMessage, 5000);
    }
    public void ContactListView_Loaded(object sender, RoutedEventArgs e)
    {
        if (!ViewModel.IsBackFromDetails) return;

        var listView = (ListView)sender;
        if (listView.SelectedItem == null) return;

        listView.ScrollIntoView(listView.SelectedItem, ScrollIntoViewAlignment.Leading);
    }
}
