using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Contacts.ViewModels;
public class ContactChangedMessage : ValueChangedMessage<Contact>
{
    public ContactChangedMessage(Contact contact, string stringMessage) : base(contact)
    {
        StringMessage = stringMessage;
    }

    public string StringMessage { get; }
}
