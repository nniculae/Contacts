using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Contacts.Views;

public sealed partial class ContactDetailPage : Page
{
    public ContactDetailPageViewModel ViewModel
    {
        get;
    }

    public ContactDetailPage()
    {
        ViewModel = App.GetService<ContactDetailPageViewModel>();
        InitializeComponent();
    }
}
