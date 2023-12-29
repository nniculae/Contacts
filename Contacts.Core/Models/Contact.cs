namespace Contacts.Core.Models;
public class Contact
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public string Name => $"{FirstName} {LastName}";
    public bool ApplyFilter(string filter) => Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase);
}
