namespace DDev.Blazor.Components.Input;

/// <summary>
/// A field for <see cref="string"/> values. (multiple lines)
/// </summary>
public partial class TextArea
{
    /// <summary>
    /// The placeholder contains hints for what a input should look like.
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// Number of visible lines. Set to <see langword="null"/> for dynamic height.
    /// </summary>
    [Parameter] public int? VisibleLines { get; set; }

    private const int VERTICAL_PADDING_PIXELS = 14;

    private string? GetUpdateHeightJs()
    {
        return VisibleLines is null
            ? $"this.style.height = '';this.style.height = this.scrollHeight - {VERTICAL_PADDING_PIXELS} + 'px'" 
            : null;
    }
}