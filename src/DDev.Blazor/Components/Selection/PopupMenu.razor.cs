namespace DDev.Blazor.Components.Selection;

public partial class PopupMenu
{
    [Parameter] public string Icon { get; set; } = "more_vert";

    [Parameter] public string Id { get; set; } = ComponentId.New();

    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    [Inject] private IJSRuntime? Js { get; set; }

    private Menu? _menu;
    private Popup? _popup;

    public async Task ShowAsync()
    {
        if (_popup is null)
            return;

        await _popup.ShowAsync(Id);
    }

    public async Task CloseAsync()
    {
        if (_popup is null)
            return;

        await _popup.CloseAsync();
    }

    private async Task HandleVisibilityChanged(bool visible)
    {
        if (visible && _menu is not null)
            await _menu.FocusAsync();

        else if (Js is not null)
            await Js.FocusAsync(Id);
    }
}