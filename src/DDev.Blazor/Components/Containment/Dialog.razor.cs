namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A dialog that can prompt the user.
/// </summary>
public partial class Dialog
{
    /// <summary>
    /// Header section.
    /// </summary>
    [Parameter] public RenderFragment<Dialog>? Header { get; set; }

    /// <summary>
    /// Main content section.
    /// </summary>
    [Parameter] public RenderFragment<Dialog>? Content { get; set; }

    /// <summary>
    /// Primary actions section.
    /// </summary>
    [Parameter] public RenderFragment<Dialog>? Actions { get; set; }

    /// <summary>
    /// Secondary actions section.
    /// </summary>
    [Parameter] public RenderFragment<Dialog>? SecondaryActions { get; set; }

    /// <summary>
    /// If provided, the referenced element is given focus when the dialog opens.
    /// </summary>
    [Parameter] public string? InitialFocusReference { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the dialog can be closed by clicking the backdrop.
    /// </summary>
    [Parameter] public bool Dismissible { get; set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    private readonly string _id = ComponentId.New();
    private Backdrop? _backdrop;
    private TaskCompletionSource<object?>? _source;

    /// <summary>
    /// Shows the dialog.
    /// </summary>
    public void Show() => ShowAsync().Discard("Failed to show dialog");

    /// <summary>
    /// Shows the dialog and waits for it to close.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the dialog is already open.</exception>
    public Task ShowAsync() => ShowAsync<object?>();

    /// <summary>
    /// Shows the dialog and waits for it to close with a result.
    /// </summary>
    /// <typeparam name="T">Type of result.</typeparam>
    /// <exception cref="InvalidOperationException">If the dialog is already open.</exception>
    public async Task<T?> ShowAsync<T>()
    {
        if (_source is not null && _source.TrySetCanceled())
            throw new InvalidOperationException("Cannot show dialog when dialog is already open");

        if (_backdrop is null)
            throw new InvalidOperationException("Is not ready.");

        _source = new TaskCompletionSource<object?>();
        await _backdrop.OpenAsync();

        // Give backdrop time to render
        await Task.Yield();

        await HandleFocusOutBottom();

        try
        {
            StateHasChanged();
            return await _source.Task is T result ? result : default;
        }
        finally
        {
            await _backdrop.CloseAsync();
        }
    }

    /// <summary>
    /// Closes the dialog. The result becomes <see langword="default"/>.
    /// </summary>
    public void Close() => CloseWithResult<object?>(null);

    /// <summary>
    /// Closes the dialog with a result.
    /// </summary>
    public void CloseWithResult<T>(T? result) => _source?.TrySetResult(result);

    private async Task HandleFocusOutTop()
    {
        if (_backdrop?.IsStackTop ?? false)
            await Js.InvokeDDevAsync("focus", "setFocusToLastChild", _id);
    }

    private async Task HandleFocusOutBottom()
    {
        if (_backdrop?.IsStackTop ?? false)
            await Js.InvokeDDevAsync("focus", "setFocusToFirstChild", _id);
    }

    private async Task HandleStackTopChanged(bool isStackTop)
    {
        if (isStackTop)
            await HandleFocusOutBottom();
    }

    private void HandleBackdropClick()
    {
        if (Dismissible)
            Close();
    }
}