using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;
using Microsoft.UI.Xaml;
using Contacts.Services;


namespace Contacts.ViewModels;

public partial class LaberCreatorViewModel(ILabelService labelService, INavigationService navigation) : ObservableObject
{
    public async Task CreateLabelAync(string labelName)
    {
        await labelService.UpsertAsync(new Label() { Name = labelName });
        //var message = $"The label {labelName} was created successfully";
        //WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(message));
    }

    public void RefreshPage()
    {
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }
}
