namespace DDev.Blazor.Components.Input;

/// <summary>
/// A regular button for taking actions.
/// </summary>
public partial class Button
{
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    [Parameter] public string? Icon { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Primary { get; set; }

    [Parameter] public bool? Selected { get; set; }

    [Parameter] public string? Tooltip { get; set; }

    [Parameter] public string? Id { get; set; } = ComponentId.New();
}