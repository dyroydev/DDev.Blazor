using System.Globalization;

namespace DDev.Blazor.Extensions;

/// <summary>
/// Usefull extension for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns <paramref name="value"/> but the first character is in uppercase using <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    public static string ToTitle(this string value)
    {
        return value.ToTitle(CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns <paramref name="value"/> but the first character is in uppercase using <paramref name="culture"/>.
    /// </summary>
    public static string ToTitle(this string value, CultureInfo? culture)
    {
        return char.ToUpper(value[0], culture ?? CultureInfo.InvariantCulture) + value[1..];
    }

    /// <summary>
    /// Returns <paramref name="value"/> but the first character is in uppercase using <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static string ToTitleInvariant(this string value)
    {
        return value.ToTitle(CultureInfo.InvariantCulture);
    }
}