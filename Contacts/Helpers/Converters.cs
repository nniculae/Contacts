using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Contacts.Helpers;

/// <summary>
/// Provides static methods for use in x:Bind function binding to convert bound values to the required value.
/// </summary>
public static class Converters
{
    /// <summary>
    /// Returns the reverse of the provided value.
    /// </summary>
    public static bool Not(bool value) => !value;

    /// <summary>
    /// Returns true if the specified value is not null; otherwise, returns false.
    /// </summary>
    public static bool IsNotNull(object value) => value != null;

    /// <summary>
    /// Returns Visibility.Collapsed if the specified value is true; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIf(bool value) =>
        value ? Visibility.Collapsed : Visibility.Visible;
    public static Visibility CollapsedIfNot(bool value) =>
        value ? Visibility.Visible : Visibility.Collapsed;

    public static Visibility ToggleVisibility(Visibility visibility) =>
        visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
    /// <summary>
    /// Returns Visibility.Collapsed if the specified value is null; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIfNull(object value) =>
        value == null ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    /// Returns Visibility.Collapsed if the specified string is null or empty; otherwise, returns Visibility.Visible.
    /// </summary>
    public static Visibility CollapsedIfNullOrEmpty(string value) =>
        string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;

   

    public static Brush StringToBrush(string value)
    {
        int hue = GenerateHue(value);
        Color c = CommunityToolkit.WinUI.Helpers.ColorHelper.FromHsl(hue, 30, 85, 0.5);
        return new SolidColorBrush(c);
    }

    private static int GenerateHue(string name)
    {
        return Math.Abs(name.GetHashCode()) % 360;
    }

    public static bool ToBool(Visibility visibility) => visibility == Visibility.Visible;
}
