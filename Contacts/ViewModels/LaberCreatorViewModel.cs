using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Core.Contracts.Services;

namespace Contacts.ViewModels;

public partial class LaberCreatorViewModel(ILabelService labelService, INavigationService navigation, IDialogService dialogService) : ObservableObject
{

    [ObservableProperty]
    private string _error = string.Empty;
    [ObservableProperty]
    private bool _showError;
    [ObservableProperty]
    private bool _cancelClose = false;


    [RelayCommand]
    public async Task ShowCreateDialog()
    {
        //await dialogService.ShowContentDialog();
    }


    [RelayCommand]
    public async Task IsLabelValidAsync(object labelName)
    {
        var name = (string)labelName;

        if (string.IsNullOrWhiteSpace(name))
        {
            Error = "Label name is required";
            ShowError = true;
            CancelClose = true;

        }
        else if (!await IsLabelUnique(name))
        {
            Error = $"The label '{name}' already exists";
            ShowError = true;
            CancelClose = true;

        }
        else
        {
            Error = string.Empty;
            ShowError = false;
            CancelClose = false;
        }

    }

    public async Task CreateLabelAync(string labelName)
    {
        await labelService.UpsertAsync(new Label() { Name = labelName });
        var message = $"The label '{labelName}' was created successfully";
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(message));
    }



    public async Task<bool> IsLabelUnique(string labelName)
    {
        var result = await labelService.GetLabelByNameAsync(labelName);

        return result == null;
    }

    public void RefreshPage()
    {
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }
}
