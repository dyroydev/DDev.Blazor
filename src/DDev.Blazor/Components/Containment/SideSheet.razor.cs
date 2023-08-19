namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A sheet that floats in from the right.
/// </summary>
public partial class SideSheet
{
    [Parameter] public string? Icon { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public string? IconTooltip { get; set; }

    [Parameter] public EventCallback OnClickIcon { get; set; }

    [Parameter] public RenderFragment? Content { get; set; }

    [Parameter] public RenderFragment? Actions { get; set; }

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