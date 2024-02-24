using Contacts.Models;
using System.Collections.ObjectModel;

namespace Contacts.Contracts.Services;

// https://mzikmund.dev/blog/using-appsettings-json-in-uwp
public interface IDatabaseFileService
{
    void Backup();
    void Delete(string backupFullFileName);
    List<BackupFile> GetAllBackups();
    void Restore(string backupFullFileName);
}
