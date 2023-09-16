namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A sheet that floats in from the bottom.
/// </summary>
public partial class BottomSheet
{
    /// <summary>
    /// Content of sheet.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Backdrop? _backdrop;
    private TaskCompletionSource? _openTaskSource;

    /// <summary>
    /// Shows the sheet.
    /// </summary>
    public void Show() => ShowAsync().Discard("Failed to open sheet.");

    /// <summary>
    /// Shows the sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when the sheet is closed.</returns>
    public async Task ShowAsync()
    {
        _backdrop?.Open();
        _openTaskSource?.TrySetResult();
        _openTaskSource = new TaskCompletionSource();
        await _openTaskSource.Task;
        _backdrop?.Close();
    }

    /// <summary>
    /// Closes the sheet if its open.
    /// </summary>
    public void Close()
    {
        _openTaskSource?.TrySetResult();
    }
}
