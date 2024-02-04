namespace Contacts.Core.Models;
public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Contact> Contacts { get;} = [];
    public List<ContactLabel> ContactLabels { get; } = [];
}
// https://learn.microsoft.com/en-us/answers/questions/1357753/seed-data-without-entity-(join-table)
// https://stenbrinke.nl/blog/taking-ef-core-data-seeding-to-the-next-level-with-bogus/