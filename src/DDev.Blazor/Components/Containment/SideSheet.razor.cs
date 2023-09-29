namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A sheet that floats in from the right.
/// </summary>
public partial class SideSheet
{
    /// <summary>
    /// Icon next to <see cref="Title"/>.
    /// </summary>
    [Parameter] public string? Icon { get; set; }

    /// <summary>
    /// Title in header of sheet.
    /// </summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Tooltip for <see cref="Icon"/>.
    /// </summary>
    [Parameter] public string? IconTooltip { get; set; }

    /// <summary>
    /// Callback invoked when the user clicks <see cref="Icon"/>.
    /// </summary>
    [Parameter] public EventCallback OnClickIcon { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the sheet is achored to the right.
    /// </summary>
    [Parameter] public bool RightAnchored { get; set; }

    /// <summary>
    /// Main content for sheet.
    /// </summary>
    [Parameter] public RenderFragment? Content { get; set; }

    /// <summary>
    /// Should contain buttons with actions for aborting or completing the sheet.
    /// </summary>
    [Parameter] public RenderFragment? Actions { get; set; }

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
        _backdrop?.OpenAsync();
        _openTaskSource?.TrySetResult();
        _openTaskSource = new TaskCompletionSource();
        await _openTaskSource.Task;
        _backdrop?.CloseAsync();
    }

    /// <summary>
    /// Cloes the sheet if its open.
    /// </summary>
    public void Close()
    {
        _openTaskSource?.TrySetResult();
    }
}