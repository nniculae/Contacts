using Contacts.Contracts.Services;
using Contacts.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Contacts.Services;
public class DialogService : IDialogService
{
    public async Task<string> InputTextDialogAsync(Func<string, Task<string>> save, string title = "Hier goes the title", string defaultText = "")
    {
        var xamlRoot = App.MainWindow.Content.XamlRoot;

        var textBoxEx = new TextBoxEx()
        {
            Text = defaultText,
        };

        textBoxEx.GotFocus += (sender, e) =>
        {
            textBoxEx.Errors = "";
            textBoxEx.ErrorsVisibility = Visibility.Collapsed;
        };

        var dialog = new ContentDialog()
        {
            Title = title,
            Content = textBoxEx,
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Cancel",
            XamlRoot = xamlRoot,
            RequestedTheme = ((FrameworkElement) App.MainWindow.Content).ActualTheme,
        };

        dialog.PrimaryButtonClick += async (sender, args) =>
        {
            var result = await save(textBoxEx.Text);

            if (result != string.Empty)
            {
                textBoxEx.ErrorsVisibility = Visibility.Visible;
                textBoxEx.Errors = result;
                args.Cancel = true;
            }
        };

        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            return textBoxEx.Text;
        }
        else
        {
            return string.Empty;
        }
    }

    public async Task<bool?> ConfirmationDialogAsync(string title)
    {
        return await ConfirmationDialogAsync(title, "Yes", string.Empty, "Cancel");
    }

    public  async Task<bool?> ConfirmationDialogAsync(string title, string yesButtonText, string noButtonText, string cancelButtonText)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            PrimaryButtonText = yesButtonText,
            SecondaryButtonText = noButtonText,
            CloseButtonText = cancelButtonText,
            XamlRoot = App.MainWindow.Content.XamlRoot,
            RequestedTheme = ((FrameworkElement)App.MainWindow.Content).ActualTheme
        };
        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.None)
        {
            return null;
        }

        return result == ContentDialogResult.Primary;
    }
}
