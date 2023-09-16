namespace DDev.Blazor.Components.Input;

/// <summary>
/// A action represented by an icon.
/// </summary>
public partial class IconButton
{
    /// <summary>
    /// Icon representing the action.
    /// </summary>
    [Parameter, EditorRequired] public string Icon { get; set; } = "";
    
    /// <summary>
    /// Callback invoked when the button is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the button is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Tooltip describing what will happen when the button is clicked.
    /// </summary>
    [Parameter] public string? Hint { get; set; }

    /// <summary>
    /// Value for <c>"id"</c>-attribute on the button element.
    /// </summary>
    [Parameter] public string? Id { get; set; } = ComponentId.New();
}