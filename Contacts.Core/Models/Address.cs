namespace Contacts.Core.Models;
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public Contact? Contact { get; set; }
    public int ContactId { get; set; }
}
