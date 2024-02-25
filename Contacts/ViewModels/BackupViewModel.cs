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

    public void OnNavigatedTo(object parameter)
    {
        SetBackupCollection();
    }

    // TODO: IsRunning is not needed anymore because of using Overlay, so remove it. 

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotRunning))]
    private bool _isRunning = false;

    public bool IsNotRunning => !IsRunning;


    private void SetBackupCollection()
    {
        BackupFiles.Clear();
        var backups = databaseFileService.GetAllBackups();
        foreach (var fileBackup in backups)
        {
            BackupFiles.Add(fileBackup);
        }
    }


    private void ShowOverlay(bool show)
    {
        Messenger.Send(new ValueChangedMessage<bool>(show));
    }

    public async Task Backup()
    {
        IsRunning = true;
        ShowOverlay(true);

        await Task.Run(() => databaseFileService.Backup());
        ShowOverlay(false);
        SetBackupCollection();
        var message = "Database was backed up successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));

        IsRunning = false;
    }



    [RelayCommand]
    public async Task RestoreAsync(string backupFullFileName)
    {
        IsRunning = true;
        ShowOverlay(true);

        await Task.Run(() => databaseFileService.Restore(backupFullFileName));
        ShowOverlay(false);
        var message = "Database was restored successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));

        IsRunning = false;
        navigation.NavigateTo(typeof(ContactListPageViewModel).FullName!, "ListContactsInit");
    }

    [RelayCommand]
    public async Task DeleteBackupAsync(string backupFullFileName)
    {
        IsRunning = true;
        ShowOverlay(true);
        await Task.Delay(5000);
        await Task.Run(() => databaseFileService.Delete(backupFullFileName));
        ShowOverlay(false);
        SetBackupCollection();
        var message = "The backup was removed successfully";
        Messenger.Send(new ValueChangedMessage<string>(message));
        IsRunning = false;

    }
    public void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

}
