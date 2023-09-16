using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace DDev.Blazor.Components.Selection;

public partial class TemplatePopupMenu<T>
{
    /// <inheritdoc cref="Menu.ChildContent" />
    [Parameter, EditorRequired] public RenderFragment<T>? ChildContent { get; set; }

    /// <inheritdoc cref="Popup.VisibleChanged"/>
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

    /// <inheritdoc cref="Menu.OnClickOption" />
    [Parameter] public EventCallback<Option> OnClickOption { get; set; }

    /// <inheritdoc cref="Popup.PlacementWidth"/>
    [Parameter] public bool PlacementWidth { get; set; }

    private T? _context;
    private Menu? _menu;
    private Popup? _popup;

    [MemberNotNullWhen(false, nameof(_popup))]
    [MemberNotNullWhen(false, nameof(_menu))]
    private bool IsNotReady => _popup is null || _menu is null;

    /// <summary>
    /// If <see langword="true"/>, the popup is visible.
    /// </summary>
    public bool IsVisible => _popup?.IsVisible ?? false;

    /// <inheritdoc cref="Menu.Options"/>
    public IReadOnlySet<Option> Options => _menu?.Options ?? Menu.EmptyOptions;

    /// <summary>
    /// Shows the popup at the given page-space coordinates.
    /// </summary>
    public async Task ShowAsync(T context, double pageX, double pageY)
    {
        if (IsNotReady)
            return;

        _context = context;
        await _popup.ShowAsync(pageX, pageY);
        await _menu.FocusAsync();
    }

    /// <summary>
    /// Shows the popup anchored to the given element.
    /// </summary>
    public async Task ShowAsync(T context, ElementReference element)
    {
        if (IsNotReady)
            return;

        _context = context;
        await _popup.ShowAsync(element);
        await _menu.FocusAsync();
    }

    /// <summary>
    /// Shows the popup anchored to the referenced element.
    /// </summary>
    public async Task ShowAsync(T context, string elementReference)
    {
        if (IsNotReady)
            return;

        _context = context;
        await _popup.ShowAsync(elementReference);
        await _menu.FocusAsync();
    }

    /// <summary>
    /// Closes the menu.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when the popup is closed.</returns>
    public async Task CloseAsync()
    {
        if (IsNotReady)
            return;

        await _popup.CloseAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Escape")
            await CloseAsync();
    }
}