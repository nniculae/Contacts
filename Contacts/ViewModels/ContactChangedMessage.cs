using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Contacts.ViewModels;
public class ContactChangedMessage(Contact contact, string stringMessage)
    : ValueChangedMessage<Contact>(contact)
{
    public string StringMessage { get; } = stringMessage;
}
