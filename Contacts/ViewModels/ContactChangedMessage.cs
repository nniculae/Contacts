using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Contacts.ViewModels;
public class ContactChangedMessage(int contactId, string stringMessage)
    : ValueChangedMessage<int>(contactId)
{
    public string StringMessage { get; } = stringMessage;
}
