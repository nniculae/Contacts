using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Contacts.Views;

public sealed partial class ContactsEditPage : Page
{
    public ContactsEditViewModel ViewModel
    {
        get;
    }

    public ContactsEditPage()
    {
        ViewModel = App.GetService<ContactsEditViewModel>();
        InitializeComponent();
    }
}
