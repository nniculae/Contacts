using Contacts.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Contacts.Views;

public sealed partial class HelpPage : Page
{
    public HelpViewModel ViewModel
    {
        get;
    }

    public HelpPage()
    {
        ViewModel = App.GetService<HelpViewModel>();
        InitializeComponent();
    }
}
