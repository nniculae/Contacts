using Bogus;
using Contacts.Core.Models;

namespace Contacts.Core.Fakes;

//https://stenbrinke.nl/blog/taking-ef-core-data-seeding-to-the-next-level-with-bogus/
public static class ContactGenerator
{
    private static readonly Faker<Contact> ContactFaker = new();
    private static readonly Faker<Address> AddressFaker = new();
    private static readonly Faker<Label> LabelFaker = new();
    private static readonly Faker<ContactLabel> ContactLabelFaker = new();

    public static List<Contact> Contacts { get; set; } = null!;
    public static List<Address> Addresses { get; set; } = null!;
    public static List<Label> Labels { get; set; } = null!;
    public static List<ContactLabel> ContactLabels { get; set; } = null!;
   

    public static void Init()
    {
        const int numOfSeeds = 200;

        var contactId = 1;
        ContactFaker
                .UseSeed(6666)
                .RuleFor(c => c.Id, _ => contactId++)
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber());
        Contacts = ContactFaker.Generate(numOfSeeds);

        var contactIds = new Queue<int>(Enumerable.Range(1, numOfSeeds));
        var addressId = 1;
        AddressFaker
                .UseSeed(6666)
                .RuleFor(c => c.Id, _ => addressId++)
                .RuleFor(c => c.Street, f => f.Address.StreetName())
                .RuleFor(c => c.Number, f => f.Address.BuildingNumber())
                .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.ContactId, _ => contactIds.Dequeue())
                ;
        Addresses = AddressFaker.Generate(numOfSeeds);


        var labelId = 1;
        LabelFaker
            .UseSeed(6666)
            .RuleFor(l => l.Id, _ => labelId++)
            .RuleFor(l => l.Name, f => f.Music.Genre()
            );

        Labels = LabelFaker.Generate(20);


        ContactLabels = GenerateContactLabels(100, Contacts, Labels);




    }

    private static List<ContactLabel> GenerateContactLabels(
        int amount,
        IEnumerable<Contact> contacts,
        IEnumerable<Label> labels)
    {
        // Now we set up the faker for our join table.
        // We do this by grabbing a random product and category that were generated.
        var contactLabelsFaker = new Faker<ContactLabel>()
            .RuleFor(x => x.ContactId, f => f.PickRandom(contacts).Id)
            .RuleFor(x => x.LabelId, f => f.PickRandom(labels).Id);

        var contactLabels = Enumerable.Range(1, amount)
            .Select(i => SeedRow(contactLabelsFaker, i))
            // We do this GroupBy() + Select() to remove the duplicates
            // from the generated join table entities
            .GroupBy(x => new { x.ContactId, x.LabelId })
            .Select(x => x.First())
            .ToList();

        return contactLabels;
    }

    private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class
    {
        var recordRow = faker.UseSeed(rowId).Generate();
        return recordRow;
    }
}
