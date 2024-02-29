
namespace Contacts.Contracts.Services;
public interface IDialogService
{
    Task<bool?> ConfirmationDialogAsync(string title);
    Task<string> InputTextDialogAsync(Func<string, Task<string>> save, string title = "Hier goes the title", string defaultText = "");
}
