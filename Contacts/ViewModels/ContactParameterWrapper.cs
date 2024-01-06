namespace Contacts.ViewModels;
public sealed class ContactParameterWrapper(Contact Contact, string infoBarMessage = "")
{
    public Contact Contact { get; } = Contact;
    public string InfoBarMessage { get; } = infoBarMessage;
}
