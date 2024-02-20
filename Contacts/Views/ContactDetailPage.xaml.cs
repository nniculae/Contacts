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

    // it is raise when i click the manage(edit) button
    private void AllLabelsListView_Loaded(object sender, RoutedEventArgs e)
    {
        SelectItems();
    }

    private void SelectItems()
    {
        
        foreach (var label in ViewModel.ContactLabels)
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
                ViewModel.ContactLabels.OrderBy(x => x.Id),
                EqualityComparer<Label>.Create(
                    (x, y) => x.Id.CompareTo(y.Id) == 0)
                );
        
      ViewModel.AreSelectedLabelsDifferent = !areEquivalent ;
      
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

        Label l =   ViewModel.CreateLabelInMemory(labelName);
        AllLabelsListView.SelectedItems.Add(l);



    }
    public void ApplyLabelChanges_Click(object sender, RoutedEventArgs e)
    {
        
        ViewModel.ContactLabels.Clear();

        foreach (var item in AllLabelsListView.SelectedItems)
        {
            ViewModel.ContactLabels.Add((Label)item);
        }
       
        ContactLabelFlyout.Hide();
    }
}
