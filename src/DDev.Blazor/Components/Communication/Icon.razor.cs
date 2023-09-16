namespace DDev.Blazor.Components.Communication;

/// <summary>
/// Icons represent concepts, objects, or actions, and have semantic purpose within a layout. They should always be recognizable, functional, and easily understood.
/// </summary>
public partial class Icon
{
    /// <summary>
    /// Name of icon. See <a href="https://fonts.google.com/icons">Google Material Symbols</a> for names.
    /// </summary>
    [Parameter, EditorRequired] public string Name { get; set; } = "";

    /// <summary>
    /// Callback invoked when the icon is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Tooltip informing what the icon represents.
    /// </summary>
    [Parameter] public string? Hint { get; set; }

    /// <summary>
    /// Optional override for size of the icon. Value is a CSS length and must include a unit.
    /// </summary>
    /// <remarks>Default is <c>1em</c>.</remarks>
    [Parameter] public string? Size { get; set; }

    /// <summary>
    /// Value given to <c>"id"</c>-attribute on icon element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();
}