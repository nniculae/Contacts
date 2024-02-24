namespace Contacts.Models;

public class LocalSettingsOptions
{
    public string? ApplicationDataFolder { get; set; }
    public string? LocalSettingsFile { get; set; }
    public string DatabaseFileName { get; set; } = null!;
    public string BackupsFolder { get; set; } = null!;
    public string DatabaseFolder { get; set; } = null!;
    public string AssetsDatabaseFolder { get; set; } = null!;
}
