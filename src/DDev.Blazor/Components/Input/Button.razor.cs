namespace DDev.Blazor.Components.Input;

/// <summary>
/// A regular button for taking actions.
/// </summary>
public partial class Button
{
    /// <summary>
    /// Content of button. Should mainly be text.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback invoked when the button is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Icon rendered to the left of the content.
    /// </summary>
    [Parameter] public string? Icon { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the button is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the button is a primary button.
    /// </summary>
    [Parameter] public bool Primary { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the button is a dangerous button.
    /// </summary>
    [Parameter] public bool Danger { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the button is selected.
    /// </summary>
    [Parameter] public bool? Selected { get; set; }

    /// <summary>
    /// Tooltip descibing what will happen if the user clicks the button.
    /// </summary>
    [Parameter] public string? Hint { get; set; }

    /// <summary>
    /// Id of the button element.
    /// </summary>
    [Parameter] public string? Id { get; set; } = ComponentId.New();
}