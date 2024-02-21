namespace Contacts.Core.Models;
public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Contact> Contacts { get;} = [];
    public List<ContactLabel> ContactLabels { get; } = [];
}
