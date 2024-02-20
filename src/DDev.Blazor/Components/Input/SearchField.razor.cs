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
    [Parameter] public EventCallback<SearchFieldArgs> OnSearch { get; set; }

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

    private readonly Timer _searchTimer = new() { AutoReset = false, Interval = 60_000 / (AssumedWordsPerMinute * AssumedAverageWordLength) };
    private CancellationTokenSource? _cancelTokenSource;
    private string _previousSearchAttempt = "";

    /// <inheritdoc />
    protected override void OnValueChanged()
    {
        if (AutoSearch)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Use(_searchTimer).Elapsed += OnTimerElapsed;
        Use(KeyBinds.Create(Id))
            .On("Enter", () => TrySearch())
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
            await InvokeAsync(TrySearch);
        }
        catch (Exception exception)
        {
            await DispatchExceptionAsync(exception);
        }
    }

    private async Task TrySearch()
    {
        var value = Value ?? "";

        if (string.Equals(value, _previousSearchAttempt))
            return;

        _previousSearchAttempt = value;

        if (value.Length < MinSearchLength)
            return;


        _cancelTokenSource?.Cancel();
        _cancelTokenSource?.Dispose();
        _cancelTokenSource = new CancellationTokenSource();

       await OnSearch.InvokeAsync(new ()
       {
           Text = value,
           CancellationToken = _cancelTokenSource.Token
       });
    }
}

/// <summary>
/// Search arguments from a <see cref="SearchField"/>.
/// </summary>
public class SearchFieldArgs
{
    /// <summary>
    /// The text value that was searched.
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Token that is cancelled when a new search is made.
    /// </summary>
    public required CancellationToken CancellationToken { get; set; }
}