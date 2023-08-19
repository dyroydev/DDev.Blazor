namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Defines a popup surface.
/// </summary>
public partial class Popup : IDisposable
{
    /// <summary>
    /// Popup content.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback invoked when the visibility of the popup has changed.
    /// </summary>
    [Parameter] public EventCallback<bool> VisibilityChanged { get; set; }

    /// <summary>
    /// Optional override for the id-attribute on the popup element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    [Inject] IJSRuntime Js { get; set; } = null!;

    private IDisposable? _onScrollListener;
    private ElementReference _popupElement;
    private bool _isVisible;
    private double _x;
    private double _y;
    private bool _fullX;
    private bool _fullY;

    /// <summary>
    /// Closes the popup.
    /// </summary>
    public async Task CloseAsync()
    {
        if (_isVisible is false)
            return;

        _isVisible = false;
        StateHasChanged();
        await VisibilityChanged.InvokeAsync(false);
    }

    /// <summary>
    /// Shows the popup at the given page-space coordinates.
    /// </summary>
    public async Task ShowAsync(double pageX, double pageY)
    {
        await ShowAsync(new PageRectangle(pageX, pageY, 0, 0));
    }

    /// <summary>
    /// Shows the popup anchored to the given element.
    /// </summary>
    public async Task ShowAsync(ElementReference element)
    {
        await ShowReferenceAsync(element);
    }

    /// <summary>
    /// Shows the popup anchored to the referenced element.
    /// </summary>
    public async Task ShowAsync(string elementReference)
    {
        await ShowReferenceAsync(elementReference);
    }

    private async Task ShowAsync(PageRectangle placement)
    {
        await using var popupModule = await Js!.OpenDDevJsModule("bounds");

        var viewport = await popupModule.InvokeAsync<PageRectangle>("getViewportBoundingRectangle");
        var popup = await popupModule.InvokeAsync<PageRectangle>("getBoundingRectangle", _popupElement);
        popup = popup with { Width = 200 };

        _fullX = 
            SetIfValid(ref _x, 'x', placement.X, popup.Width, viewport.X, viewport.Width) is false &&
            SetIfValid(ref _x, 'x', placement.X + placement.Width - popup.Width, popup.Width, viewport.X, viewport.Width) is false;

        _fullY =
            SetIfValid(ref _y, 'y', placement.Y + placement.Height, popup.Height, viewport.Y, viewport.Height) is false &&
            SetIfValid(ref _y, 'y', placement.Y - popup.Height, popup.Height, viewport.Y, viewport.Height) is false;


        _isVisible = true;
        StateHasChanged();

        await VisibilityChanged.InvokeAsync(true);
    }

    protected override async Task OnInitializedAsync()
    {
        _onScrollListener = await Js.OnEvent("scroll", CloseAsync);
    }

    void IDisposable.Dispose()
    {
        _onScrollListener?.Dispose();
    }

    private async Task HandleFocusOut()
    {
        await using var focusModule = await Js.OpenDDevJsModule("focus");

        if (await focusModule.InvokeAsync<bool>("hasFocus", _popupElement))
            return;

        await CloseAsync();
    }

    private static bool SetIfValid(ref double value, char label, double offset, double size, double viewportOffset, double viewportSize)
    {
        var tooSmall = offset < 0;
        var tooLarge = offset + size > viewportSize + viewportOffset - 0;

        if (tooSmall || tooLarge)
            return false;

        value = offset - viewportOffset;
        return true;
    }

    private async Task ShowReferenceAsync(object? reference)
    {
        var boundingRectangle = await Js.InvokeDDevAsync<PageRectangle>("bounds", "getBoundingRectangle", reference);
        await ShowAsync(boundingRectangle);
    }

    private record PageRectangle(double X, double Y, double Width, double Height);
}