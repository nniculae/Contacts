using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Contacts.Controls;

public sealed class TextBoxEx : TextBox
{
    private readonly Grid gridErrorsContainer = new() { Margin = new Thickness(0, 8, 0, 0) };
    private readonly TextBlock errors = new() { TextWrapping = TextWrapping.WrapWholeWords };

    public TextBoxEx()
    {
        DefaultStyleKey = typeof(TextBoxEx);
    }
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        gridErrorsContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        gridErrorsContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        errors.Foreground = new SolidColorBrush() { Color = Microsoft.UI.Colors.Red };

        var errorIcon = new FontIcon
        {
            Glyph = "\uE783",
            Margin = new Thickness(0, 0, 4, 0),
            Foreground = new SolidColorBrush() { Color = Microsoft.UI.Colors.Red }
        };

        Grid.SetColumn(errorIcon, 0);
        Grid.SetColumn(errors, 1);

        gridErrorsContainer.Children.Add(errorIcon);
        gridErrorsContainer.Children.Add(errors);

        Description = gridErrorsContainer;
    }

    public string Errors
    {
        get { return errors.Text; }
        set { errors.Text = value; }
    }

    public Visibility ErrorsVisibility
    {
        get { return gridErrorsContainer.Visibility; }
        set { gridErrorsContainer.Visibility = value; }
    }
}