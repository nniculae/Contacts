namespace Contacts.Core.Models;
public class Label: IComparable<Label>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Contact> Contacts { get;} = [];
    public List<ContactLabel> ContactLabels { get; } = [];

    public int CompareTo(Label? other)
    {
        if (other == null) return 1;

        return Name.CompareTo(other.Name);
    }
}
