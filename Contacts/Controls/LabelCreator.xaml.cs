using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Extensions;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace Contacts.Controls;

public sealed partial class LabelCreator : UserControl
{
    public LaberCreatorViewModel ViewModel { get; }

    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    public LabelCreator()
    {
        ViewModel = App.GetService<LaberCreatorViewModel>();
        this.InitializeComponent();

    }
    private async void CreateLabelButton_Click(object sender, RoutedEventArgs e)
    {


    }
    private void LabelCreator_LabelContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        //ViewModel.CancelClose = false;
        //ViewModel.Error = string.Empty;
        ViewModel.ShowError = false;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {

        await dispatcherQueue.EnqueueCustomAsync(() =>
        {

            // Register a message in some module
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<Type>>(this, async (r, m) =>
            {
                var labelName = await LabelCreator_LabelContentDialog.ShowAsync();

                if (string.IsNullOrEmpty(ViewModel.Error))
                {
                    await ViewModel.CreateLabelAync(labelName);
                    ViewModel.RefreshPage();
                }

            });

        });


    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<Type>>(this);
    }
}
