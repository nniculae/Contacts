using Bogus;
using Contacts.Core.Models;

namespace Contacts.Core.Fakes;
public static class ContactGenerator
{

    public static Faker<Contact> ContactFaker;
    public static Faker<Address> AddressFaker;

    public static List<Contact> Contacts { get; set; }
    public static List<Address> Addresses { get; set; }

    public static void Init()
    {
        var numOfSeeds = 1000;

        var contactId = 1;
        ContactFaker = new Faker<Contact>()
                .UseSeed(6666)
                .RuleFor(c => c.Id, f => contactId++)
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber());
        Contacts = ContactFaker.Generate(numOfSeeds);


        var contactIds = new Queue<int>(Enumerable.Range(1, numOfSeeds));
        var addressId = 1;
        AddressFaker = new Faker<Address>()
                .UseSeed(6666)
                .RuleFor(c => c.Id, f => addressId++)
                .RuleFor(c => c.Street, f => f.Address.StreetName())
                .RuleFor(c => c.Number, f => f.Address.BuildingNumber())
                .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.ContactId, f => contactIds.Dequeue())
                ;
        Addresses = AddressFaker.Generate(numOfSeeds);

    }
}
