namespace DDev.Blazor.Components.Selection;

/// <summary>
/// A menu that pops up anchored to a button.
/// </summary>
public partial class PopupMenuButton
{
    /// <summary>
    /// Icon in button.
    /// </summary>
    [Parameter] public string Icon { get; set; } = "more_vert";

    /// <summary>
    /// Value for <c>"id"</c>-attribute on button element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    /// <inheritdoc cref="Menu.ChildContent" />
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    [Inject] private IJSRuntime? Js { get; set; }

    private PopupMenu? _popupMenu;

    /// <summary>
    /// Shows the popup menu.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when the popup is open.</returns>
    public async Task ShowAsync()
    {
        if (_popupMenu is null)
            return;

        await _popupMenu.ShowAsync(Id);
    }

    /// <summary>
    /// Closes the menu.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when the popup is closed.</returns>
    public async Task CloseAsync()
    {
        if (_popupMenu is null)
            return;

        await _popupMenu.CloseAsync();
    }

    private async Task HandleVisibleChanged(bool visible)
    {
        if (visible is false && Js is not null)
            await Js.FocusAsync(Id);
    }
}