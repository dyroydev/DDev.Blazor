using System.Diagnostics.CodeAnalysis;

namespace DDev.Blazor.Components.Selection;

/// <summary>
/// A <see cref="Menu"/> contained in a <see cref="Popup"/>.
/// </summary>
public partial class PopupMenu
{
    /// <inheritdoc cref="Menu.ChildContent" />
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="Popup.VisibleChanged"/>
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    /// <inheritdoc cref="Menu.OnClickOption" />
    [Parameter] public EventCallback<Option> OnClickOption { get; set; }

    /// <inheritdoc cref="Popup.PlacementWidth"/>
    [Parameter] public bool PlacementWidth { get; set; }

    private TemplatePopupMenu<object>? _popupMenu;

    /// <summary>
    /// If <see langword="true"/>, the popup is visible.
    /// </summary>
    public bool IsVisible => _popupMenu?.IsVisible ?? false;

    /// <inheritdoc cref="Menu.Options"/>
    public IReadOnlySet<Option> Options => _popupMenu?.Options ?? Menu.EmptyOptions;

    /// <summary>
    /// Shows the popup at the given page-space coordinates.
    /// </summary>
    public async Task ShowAsync(double pageX, double pageY)
    {
        if (_popupMenu is null)
            return;

        await _popupMenu.ShowAsync(null!, pageX, pageY);
    }

    /// <summary>
    /// Shows the popup anchored to the given element.
    /// </summary>
    public async Task ShowAsync(ElementReference element)
    {
        if (_popupMenu is null)
            return;

        await _popupMenu.ShowAsync(null!, element);
    }

    /// <summary>
    /// Shows the popup anchored to the referenced element.
    /// </summary>
    public async Task ShowAsync(string elementReference)
    {
        if (_popupMenu is null)
            return;

        await _popupMenu.ShowAsync(null!, elementReference);
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
}