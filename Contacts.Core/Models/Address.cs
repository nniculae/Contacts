namespace Contacts.Core.Models;
public class Address
{
    public int Id { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; } 
    public Contact? Contact { get; set; }
    public int ContactId { get; set; }
}
