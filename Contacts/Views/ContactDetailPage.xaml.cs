using Contacts.Behaviors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Contacts.Services;

namespace Contacts.Views;
public sealed partial class ContactDetailPage : Page
{
    public ContactDetailPageViewModel ViewModel { get; }
    public ContactDetailPage()
    {
        ViewModel = App.GetService<ContactDetailPageViewModel>();
        InitializeComponent();
        NavigationViewHeaderBehavior.SetHeaderMode(this, NavigationViewHeaderMode.Never);
    }

    private void AllLabelsListView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (var label in ViewModel.Contact.Labels)
        {
            foreach (var label2 in AllLabelsListView.Items)
            {
                if (label.Id == ((Label)label2).Id)
                {
                    AllLabelsListView.SelectedItems.Add(label2);
                }
            }
        }
    }

    private void AllLabelsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        var allLabels = AllLabelsListView.SelectedItems.Cast<Label>().ToList();

        bool areEquivalent = allLabels.OrderBy(x => x.Id)
            .SequenceEqual(
                ViewModel.Contact.Labels.OrderBy(x => x.Id),
                EqualityComparer<Label>.Create(
                    (x, y) => x.Id.CompareTo(y.Id) == 0)
                );

      ViewModel.AreSelectedLabelsDifferent = !areEquivalent;
      ViewModel.SelectedLabels = allLabels;  
    }

    private async void createLabelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ContactLabelFlyout.Hide();
        var element = (FrameworkElement)App.MainWindow.Content;
        var labelName = await element.InputStringDialogAsync(
                "Create new label",
                "");
        if (string.IsNullOrEmpty(labelName))
            return;

        await ViewModel.CreateLabelAsync(labelName);
        
    }
}
