using Bogus;
using Contacts.Core.Models;

namespace Contacts.Core.Fakes;
public static class ContactGenerator
{
    private static readonly Faker<Contact> ContactFaker = new();
    private static readonly Faker<Address> AddressFaker = new();

    public static List<Contact> Contacts { get; set; } = null!;
    public static List<Address> Addresses { get; set; } = null!;

    public static void Init()
    {
        const int numOfSeeds = 1000;

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
    }
}
