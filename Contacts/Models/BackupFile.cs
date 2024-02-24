namespace Contacts.Models;
public class BackupFile
{
    public int Id { get; set; }
    public string FullFileName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
