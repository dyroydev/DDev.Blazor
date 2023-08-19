namespace DDev.Blazor.Components.Input;

public partial class IconButton
{
    [Parameter, EditorRequired] public string Icon { get; set; } = "";
    
    [Parameter] public EventCallback OnClick { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public string? Tooltip { get; set; }

    [Parameter] public string? Id { get; set; } = ComponentId.New();
}