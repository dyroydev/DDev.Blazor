namespace DDev.Blazor.Services;

/// <summary>
/// A modifier-key on the keyboard.
/// </summary>
[Flags]
public enum ModifierKey
{
    /// <summary>
    /// No control keys.
    /// </summary>
    None = 0,
    /// <summary>
    /// Alt-key on windows, Option-key on mac.
    /// </summary>
    Alt = 1,
    /// <summary>
    /// Control-key on windows, Command-key on mac.
    /// </summary>
    Control = 2,
    /// <summary>
    /// Shift-key.
    /// </summary>
    Shift = 4,
    /// <summary>
    /// Windows-key on windows, Command-key on mac.
    /// </summary>
    Meta = 8,
}
