namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A sheet that floats in from the bottom.
/// </summary>
public partial class BottomSheet
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Backdrop? _backdrop;
    private TaskCompletionSource? _openTaskSource;

    public void Show() => ShowAsync().Discard("Failed to open sheet.");

    public async Task ShowAsync()
    {
        _backdrop?.Open();
        _openTaskSource?.TrySetResult();
        _openTaskSource = new TaskCompletionSource();
        await _openTaskSource.Task;
        _backdrop?.Close();
    }

    public void Close()
    {
        _openTaskSource?.TrySetResult();
    }
}
