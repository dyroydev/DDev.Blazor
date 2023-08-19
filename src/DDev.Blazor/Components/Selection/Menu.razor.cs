namespace DDev.Blazor.Components.Selection;

public partial class Menu
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string Id { get; set; } = ComponentId.New();

    [Parameter] public EventCallback OnClick { get; set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    public async Task FocusAsync()
    {
        await Js.FocusAsync($"#{Id} li:not([disabled])");
    }
}