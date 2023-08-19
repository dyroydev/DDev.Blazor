namespace DDev.Blazor.Components.Utilities;

/// <summary>
/// Creates <a href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes">aria attributes</a> from C# objects.
/// </summary>
public static class Aria
{
    /// <summary>
    /// Translates <paramref name="selected"/> to a valid value for <c>aria-selected</c>.
    /// </summary>
    /// <returns>One of <c>"true"</c>, <c>"false"</c> and <see langword="null"/>.</returns>
    public static string? Selected(bool? selected)
    {
        if (selected is null)
            return null;
        return selected.Value ? "true" : "false";
    }
}