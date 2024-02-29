using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace Contacts.Controls;

public sealed partial class LabelContentDialog : ContentDialog
{
    public LabelContentDialog()
    {
        InitializeComponent();
        XamlRoot = App.MainWindow?.Content?.XamlRoot;
        CloseButtonClick += LabelContentDialog_CloseButtonClick;
        Closing += LabelContentDialog_Closing;
    }

    private void LabelContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        CancelClose = false;
    }

    private void LabelContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        args.Cancel = CancelClose;
        TextBoxText = string.Empty;
    }


    public bool CancelClose
    {
        get { return (bool)GetValue(CancelCloseProperty); }
        set { SetValue(CancelCloseProperty, value); }
    }


    public static readonly DependencyProperty CancelCloseProperty =
        DependencyProperty.Register(nameof(CancelClose), typeof(bool), typeof(LabelContentDialog), new PropertyMetadata(false));

    public string TextBoxHeader
    {
        get { return (string)GetValue(TextBoxHeaderProperty); }
        set { SetValue(TextBoxHeaderProperty, value); }
    }

    public static readonly DependencyProperty TextBoxHeaderProperty =
        DependencyProperty.Register(nameof(TextBoxHeader), typeof(string), typeof(LabelContentDialog), new PropertyMetadata(string.Empty));

    public string TextBoxText
    {
        get { return (string)GetValue(TextBoxTextProperty); }
        set { SetValue(TextBoxTextProperty, value); }
    }

    public static readonly DependencyProperty TextBoxTextProperty =
        DependencyProperty.Register(nameof(TextBoxText), typeof(string), typeof(LabelContentDialog), new PropertyMetadata(string.Empty));

    public Visibility ValidationErrorsVisibilty
    {
        get { return (Visibility)GetValue(ValidationErrorsVisibiltyProperty); }
        set { SetValue(ValidationErrorsVisibiltyProperty, value); }
    }


    public static readonly DependencyProperty ValidationErrorsVisibiltyProperty =
        DependencyProperty.Register(nameof(ValidationErrorsVisibilty), typeof(Visibility), typeof(LabelContentDialog), new PropertyMetadata(Visibility.Collapsed));



    public string ValidationErrorsText
    {
        get { return (string)GetValue(ValidationErrorsTextProperty); }
        set { SetValue(ValidationErrorsTextProperty, value); }
    }

    public static readonly DependencyProperty ValidationErrorsTextProperty =
        DependencyProperty.Register(nameof(ValidationErrorsText), typeof(string), typeof(LabelContentDialog), new PropertyMetadata(string.Empty));

    public new async Task<string> ShowAsync()
    {
        if (await base.ShowAsync() == ContentDialogResult.Primary)
        {
            return TextBoxText;
        }
        else
        {
            return string.Empty;
        }
    }
}


