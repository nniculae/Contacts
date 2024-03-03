using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Models;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class BackupViewModel(
    IDatabaseFileService databaseFileService,
    INavigationService navigation) : ObservableRecipient, INavigationAware
{

    public ObservableCollection<BackupFile> BackupFiles { get; set; } = [];

    public async Task OnNavigatedTo(object parameter)
    {
        await SetBackupCollection();

    }

    private async Task SetBackupCollection()
    {
        BackupFiles.Clear();

        List<BackupFile> backups = [];

        await Task.Run(() =>
        {
            backups = databaseFileService.GetAllBackups();
        });

        foreach (var fileBackup in backups)
        {
            BackupFiles.Add(fileBackup);
        }
    }


    private void ShowOverlay(bool show)
    {
        Messenger.Send(new ValueChangedMessage<bool>(show));
    }

    [RelayCommand]
    public async Task BackupAsync()
    {
        ShowOverlay(true);
        await Task.Run(() => databaseFileService.Backup());
        ShowOverlay(false);
        await SetBackupCollection();
        var message = "Database was backed up successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));
    }


    [RelayCommand]
    public async Task RestoreAsync(string backupFullFileName)
    {

        ShowOverlay(true);

        await Task.Run(() => databaseFileService.Restore(backupFullFileName));
        ShowOverlay(false);
        var message = "Database was restored successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    [RelayCommand]
    public async Task DeleteBackupAsync(string backupFullFileName)
    {

        ShowOverlay(true);
        await Task.Run(() => databaseFileService.Delete(backupFullFileName));
        ShowOverlay(false);
        await SetBackupCollection();
        var message = "The backup was removed successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));

    }
    public void OnNavigatedFrom()
    {
    }
}
