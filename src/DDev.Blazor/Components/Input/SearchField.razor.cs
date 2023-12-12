using DDev.Blazor.Services;
using Timer = System.Timers.Timer;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A field for searching.
/// </summary>
public partial class SearchField : IDisposable
{
    /// <summary>
    /// Assumed number of words per minute you userbase can type.
    /// </summary>
    /// <remarks>An average person can type about 38 words per minute, this is the default value.</remarks>
    public static double AssumedWordsPerMinute { get; set; } = 38;

    /// <summary>
    /// Assumed average word length in the language of your userbase.
    /// </summary>
    /// <remarks>The default is 5.1. (Average word length in English)</remarks>
    public static double AssumedAverageWordLength { get; set; } = 5.1;

    /// <summary>
    /// Callback invoked when the user wants to search.
    /// </summary>
    [Parameter] public EventCallback<string> OnSearch { get; set; }

    /// <summary>
    /// Callback invoked when the user wants to navigate the result list.
    /// </summary>
    [Parameter] public EventCallback OnTryFocusFirstResult { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the search will be triggered as the user types.
    /// </summary>
    [Parameter] public bool AutoSearch { get; set; }

    /// <summary>
    /// Minimum number of characters before triggering a search.
    /// </summary>
    [Parameter] public int MinSearchLength { get; set; } = 0;

    [Inject] private IKeyBindingsFactory KeyBinds { get; set; } = default!;

    private readonly Timer _searchTimer = new();

    /// <inheritdoc />
    protected override void OnValueChanged()
    {
        if (AutoSearch)
        {
            _searchTimer.Stop();
            _searchTimer.Interval = 60_000 / (AssumedWordsPerMinute * AssumedAverageWordLength);
            _searchTimer.Start();
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Use(_searchTimer).Elapsed += OnTimerElapsed;
        Use(KeyBinds.ForElement(Id))
            .On("Enter", () => Search())
            .On("ArrowDown", () => OnTryFocusFirstResult.InvokeAsync());
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _searchTimer.Elapsed -= OnTimerElapsed;
    }

    private async void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            await InvokeAsync(Search);
        }
        catch (Exception exception)
        {
            await DispatchExceptionAsync(exception);
        }
    }

    private async Task Search()
    {
        var value = Value ?? "";

        if (value.Length < MinSearchLength)
            return;

       await OnSearch.InvokeAsync(value);
    }
}
