using DDev.Blazor.Internal;

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
    /// Callback invoked when the backdrop-stack has changed.
    /// </summary>
    /// <remarks>
    /// Arguments indicates if this backdrop is the top of the stack or not.<br/>
    /// You can also see <see cref="IsStackTop"/>.
    /// </remarks>
    [Parameter] public EventCallback<bool> OnStackTopChanged { get; set; }

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

    [Inject] BackdropStack Stack { get; set; } = null!;

    /// <summary>
    /// <see langword="true"/> if this backdrop is currently the topmost backdrop.
    /// </summary>
    public bool IsStackTop => _isStackTop;

    private readonly string _backdropId = ComponentId.New();
    private PortalSource? _portal;
    private bool _isOpen;
    private bool _isRendered;
    private bool _isStackTop;

    /// <summary>
    /// Opens the backdrop and shows its content.
    /// </summary>
    public async Task OpenAsync()
    {
        _isOpen = true;
        _isRendered = _isOpen;
        StateHasChanged();

        await Stack.PushAsync(this);
    }

    /// <summary>
    /// Closes the backdrop and hides its content.
    /// </summary>
    public async Task CloseAsync()
    {
        await Stack.PushAsync(this);
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

    /// <summary>
    /// Callback invoked by <see cref="BackdropStack"/> when this backdrop is at the top of the stack.
    /// </summary>
    internal async Task NotifyStackTopAsync(bool isTopOfStack)
    {
        _isStackTop = isTopOfStack;
        if (isTopOfStack)
            _portal?.MoveToEnd();

        StateHasChanged();

        await OnStackTopChanged.InvokeAsync(isTopOfStack);
    }
}