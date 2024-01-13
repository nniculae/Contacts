﻿namespace Contacts.Core.Models;
public class Contact : IComparable<Contact>
{
   
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public string Name => $"{FirstName} {LastName}";
    public string FirstLetter => FirstName[0].ToString();
    public bool ApplyFilter(string filter) => Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase);

    public int CompareTo(Contact? other)
    {
        if (other == null) return 1;

        return Id.CompareTo(other.Id);
    }
}
