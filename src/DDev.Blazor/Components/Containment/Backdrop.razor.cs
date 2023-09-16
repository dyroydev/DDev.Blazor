namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A backdrop used for modal and dialog-like components.
/// </summary>
public partial class Backdrop
{
    /// <summary>
    /// Content rendered on top of backdrop.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback invoked when the backdrop is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Callback invoked when the focus has left the backdrop content.
    /// </summary>
    [Parameter] public EventCallback OnFocusOut { get; set; }

    /// <summary>
    /// Callback invoked when the focus has left the backdrop content using <c>[SHIFT] + [TAB]</c>.
    /// </summary>
    [Parameter] public EventCallback OnFocusOutTop { get; set; }

    /// <summary>
    /// Callback invoked when the focus has left the backdrop content using <c>[TAB]</c>.
    /// </summary>
    [Parameter] public EventCallback OnFocusOutBottom { get; set; }

    [Inject] IJSRuntime? Js { get; set; }

    private readonly string _backdropId = ComponentId.New();
    private PortalSource? _portal;
    private bool _isOpen;
    private bool _isRendered;

    /// <summary>
    /// Opens the backdrop and shows its content.
    /// </summary>
    public void Open()
    {
        _portal?.MoveToEnd();
        _isOpen = true;
        _isRendered = _isOpen;
        StateHasChanged();
    }

    /// <summary>
    /// Closes the backdrop and hides its content.
    /// </summary>
    public void Close()
    {
        _isOpen = false;
        StateHasChanged();

        DisableRenderAfterTimeout().Discard("Failed to disable render");
    }

    private async Task HandleClick()
    {
        await OnClick.InvokeAsync();
    }

    private async Task HandleFocusOut()
    {
        if (await Js!.HasFocusAsync(_backdropId))
            return;

        await OnFocusOut.InvokeAsync();
    }

    private async Task HandleFocusTop()
    {
        if (OnFocusOutTop.HasDelegate)
            await OnFocusOutTop.InvokeAsync();
        else
            await HandleFocusOut();
    }

    private async Task HandleFocusBottom()
    {
        if (OnFocusOutBottom.HasDelegate)
            await OnFocusOutBottom.InvokeAsync();
        else
            await HandleFocusOut();
    }

    /// <summary>
    /// Disabled rendering after timeout if backdrop is closed.
    /// </summary>
    /// <remarks>
    /// This is to allow for an exit animation.
    /// </remarks>
    private async Task DisableRenderAfterTimeout()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(300));
        if (_isOpen is false)
        {
            _isRendered = false;
            StateHasChanged();
        }
    }
}