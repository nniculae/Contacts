using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Contracts.Services;
using Contacts.Contracts.ViewModels;
using Contacts.Models;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels;

public partial class BackupViewModel(IDatabaseFileService databaseFileService) : ObservableRecipient, INavigationAware
{

    public ObservableCollection<BackupFile> BackupFiles { get; set; } = [];

    public void OnNavigatedTo(object parameter)
    {
        SetBackupCollection();
    }


    private void SetBackupCollection()
    {
        BackupFiles.Clear();
        var backups = databaseFileService.GetAllBackups();
        foreach (var fileBackup in backups)
        {
            BackupFiles.Add(fileBackup);
        }
    }

    public async Task Backup()
    {
        await Task.Run( ()=> databaseFileService.Backup());
        SetBackupCollection();
    }

    [RelayCommand]
    public async Task RestoreAsync(string backupFullFileName)
    {
        await Task.Run(() => databaseFileService.Restore(backupFullFileName));
    }

    [RelayCommand]
    public async Task DeleteBackupAsync(string backupFullFileName)
    {
        await Task.Run(() => databaseFileService.Delete(backupFullFileName));
        SetBackupCollection();
    }
    public void OnNavigatedFrom()
    {
        //throw new NotImplementedException();
    }

}
