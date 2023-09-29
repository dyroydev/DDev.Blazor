using Microsoft.AspNetCore.Components.Web;

namespace DDev.Blazor.Components.Selection;

/// <summary>
/// Represents a option in a <see cref="Selection.Menu"/>.
/// </summary>
public partial class Option : IDisposable
{
    /// <summary>
    /// Option text.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Id given to <c>"id"</c>-attribute on <c>li</c>-element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    /// <summary>
    /// Icon for option.
    /// </summary>
    [Parameter] public string? Icon { get; set; }

    /// <summary>
    /// Hint text for the option.
    /// </summary>
    [Parameter] public string? Hint { get; set; }

    /// <summary>
    /// Hint icon for the option.
    /// </summary>
    [Parameter] public string? HintIcon { get; set; }

    /// <summary>
    /// Callback invoked when the option is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Callback invoked when the hint is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClickHint { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the option is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// An optional value attached to this option.
    /// </summary>
    [Parameter] public object? Value { get; set; }

    /// <summary>
    /// Menu that contains this option. Might be <see langword="null"/>.
    /// </summary>
    [CascadingParameter] private Menu? Menu { get; set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    /// <summary>
    /// If <see langword="true"/>, the options is rendered as selected.
    /// </summary>
    internal bool Selected { get; set; }

    private async Task HandleClick()
    {
        if (Disabled)
            return;

        await OnClick.InvokeAsync();

        if (Menu is not null)
            await Menu.NotifyClickedAsync(this);
    }

    private async Task HandleClickHint()
    {
        if (Disabled)
            return;

        await OnClickHint.InvokeAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" || args.Key == "Space")
            await HandleClick();
        else
            await HandleNavigation(args);
    }

    private async Task HandleKeyDownHint(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" || args.Key == "Space")
            await HandleClickHint();
        else
            await HandleNavigation(args);
    }

    private async Task HandleNavigation(KeyboardEventArgs args)
    {
        if (Menu is null)
            return;

        if (args.Key == "ArrowUp" || args.Key == "ArrowRight")
            await Menu.HandleNavigationAsync(this, -1);
        else if (args.Key == "ArrowDown" || args.Key == "ArrowLeft")
            await Menu.HandleNavigationAsync(this, 1);
    }

    /// <summary>
    /// Notifies the component that its state has changed.
    /// </summary>
    internal void NotifyStateHasChanged()
    {
        StateHasChanged();
    }

    void IDisposable.Dispose()
    {
        Menu?.InternalOptions.Remove(this);
    }

    /// <inheritdoc />
    protected override sealed void OnInitialized()
    {
        Menu?.InternalOptions.Add(this);
    }
}