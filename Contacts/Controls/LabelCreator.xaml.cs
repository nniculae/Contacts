using Contacts.Contracts.Services;
using Contacts.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace Contacts.Controls;

public sealed partial class LabelCreator : UserControl
{
    public LaberCreatorViewModel ViewModel { get; }
    

    public LabelCreator()
    {
        ViewModel = App.GetService<LaberCreatorViewModel>();
        this.InitializeComponent();
        
    }
    private async void CreateLabelButton_Click(object sender, RoutedEventArgs e)
    {
        var element = (FrameworkElement)App.MainWindow.Content;
        var labelName = await element.InputStringDialogAsync("Create new label", string.Empty);
        if (string.IsNullOrEmpty(labelName))
            return;
        await ViewModel.CreateLabelAync(labelName);

        ViewModel.RefreshPage();
    }
}
