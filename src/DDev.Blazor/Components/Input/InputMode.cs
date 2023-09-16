namespace DDev.Blazor.Components.Input;

/// <summary>
/// Hint allowing the browser to display the most appropriate virtial keyboard.
/// </summary>
/// <remarks>Only has an effect on devices with virtual keyboards.</remarks>
public enum InputMode
{
    /// <summary>
    /// Use normal keyboard.
    /// </summary>
    Text,
    /// <summary>
    /// Use keyboard optimized for decimal numeric values.
    /// </summary>
    Decimal,
    /// <summary>
    /// Use keyboard optimized for integer numeric values.
    /// </summary>
    Numeric,
    /// <summary>
    /// Use keyboard optimized for phone numbers.
    /// </summary>
    Telephone,
    /// <summary>
    /// Use keyboard optimized for searches.
    /// </summary>
    Search,
    /// <summary>
    /// Use keyboard optimized for email addresses.
    /// </summary>
    Email,
    /// <summary>
    /// Use keyboard optimized for URLs.
    /// </summary>
    URL
}