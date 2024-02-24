using Contacts.Contracts.Services;
using Contacts.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Contacts.Services;
public class DatabaseFileService : IDatabaseFileService
{
    private const string _defaultApplicationDataFolder = "Contacts/ApplicationData";
    
    private readonly string _localApplicationData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private string _databaseFileName;
    private string _databaseFolder;
    private string _backupsFolder;

    private readonly LocalSettingsOptions _options;
    public DatabaseFileService(IOptions<LocalSettingsOptions> options)
    {
        _options = options.Value;

        // This folder is created bij the settings
        _applicationDataFolder = Path.Combine(
             _localApplicationData,
             _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _databaseFileName = _options.DatabaseFileName;

        // This is created by sql connection
        _databaseFolder =Path.Combine(_applicationDataFolder,  _options.DatabaseFolder);

        _backupsFolder = Path.Combine(_applicationDataFolder, _options.BackupsFolder);
        if (!Directory.Exists(_backupsFolder))
        {
            Directory.CreateDirectory(_backupsFolder);
        }

    }


    public List<BackupFile> GetAllBackups()
    {


        List<BackupFile> backupFiles = [];

        DirectoryInfo info = new DirectoryInfo(_backupsFolder);

        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
        for (int i = 0; i < files.Length; i++)
        {
            backupFiles.Add(new BackupFile() {
                Id = i,
                CreatedAt = files[i].CreationTime,
                FullFileName = files[i].FullName });
        }

        return backupFiles;
    }

    public void Restore(string backupFullFileName)
    {
        SqliteConnection.ClearAllPools();
        File.Copy(
            Path.Combine(_backupsFolder, backupFullFileName),
            Path.Combine(_databaseFolder, _databaseFileName),
            true
            );
    }

    public void Delete(string backupFullFileName)
    {
        SqliteConnection.ClearAllPools();
        File.Delete(backupFullFileName );
    }

    public void Backup()
    {
        SqliteConnection.ClearAllPools();
        var dbFileNameWithDateAppended = _databaseFileName + "_" + DateTime.Now.Ticks;

        File.Copy(
            Path.Combine( _databaseFolder, _databaseFileName),
            Path.Combine(_backupsFolder, dbFileNameWithDateAppended)
            );
    }

}
