namespace DDev.Blazor.Components.Schedules;

/// <summary>
/// A source for schedule components. Used to get schedule items and give their templates.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ScheduleSource<T> : ComponentBase, IScheduleSource, IDisposable
{
    /// <summary>
    /// Template for schedule items fetched by this source.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment<T> ChildContent { get; set; } = _ => throw new InvalidOperationException($"Value for {nameof(ChildContent)} parameter must be provided.");

    /// <summary>
    /// Gets the date and time from the schedule item.
    /// </summary>
    /// <remarks>
    /// Time is ignored when <see cref="AllDay"/> returns <see langword="true"/>.
    /// </remarks>
    [Parameter, EditorRequired] public Func<T, DateTimeOffset> DateTime { get; set; } = _ => throw new InvalidOperationException($"Value for {nameof(DateTime)} parameter must be provided.");

    /// <summary>
    /// Gets the duration from the schedule item.
    /// </summary>
    /// <remarks>
    /// Ignored when <see cref="AllDay"/> returns <see langword="true"/>.
    /// </remarks>
    [Parameter] public Func<T, TimeSpan?> Duration { get; set; } = _ => null;
    
    /// <summary>
    /// Gets a boolean indicating if item is a special all-day item.
    /// </summary>
    /// <remarks>
    /// If <see langword="true"/>, time and duration are ignored.
    /// </remarks>
    [Parameter] public Func<T, bool> AllDay { get; set; } = _ => false;

    /// <summary>
    /// A source of in-memory items.
    /// </summary>
    [Parameter] public IEnumerable<T>? Source { get; set; }

    /// <summary>
    /// Callback used to get items asynchronously.
    /// </summary>
    [Parameter] public AsyncFetchDelegate? FetchAsync { get; set; }

    /// <summary>
    /// Callback used to get items.
    /// </summary>
    [Parameter] public FetchDelegate? Fetch { get; set; }

    [CascadingParameter] private IScheduleSourceContainer? Container { get; set; }

    /// <summary>
    /// Function querying for schedule items between <paramref name="inclusiveStart"/> and <paramref name="inclusiveEnd"/> asynchronously.
    /// </summary>
    /// <param name="inclusiveStart">First date to query for items.</param>
    /// <param name="inclusiveEnd">Last date to query for items.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    public delegate Task<IEnumerable<T>> AsyncFetchDelegate(DateOnly inclusiveStart, DateOnly inclusiveEnd, CancellationToken cancellationToken);

    /// <summary>
    /// Function querying for schedule items between <paramref name="inclusiveStart"/> and <paramref name="inclusiveEnd"/>.
    /// </summary>
    /// <param name="inclusiveStart">First date to query for items.</param>
    /// <param name="inclusiveEnd">Last date to query for items.</param>
    public delegate IEnumerable<T> FetchDelegate(DateOnly inclusiveStart, DateOnly inclusiveEnd);

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Container?.AddScheduleSource(this);
    }

    void IDisposable.Dispose()
    {
        Container?.RemoveScheduleSource(this);
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    async Task<IEnumerable<ScheduleSourceItem>> IScheduleSource.GetBetween(DateOnly inclusiveStart, DateOnly inclusiveEnd)
    {
        var items = new List<ScheduleSourceItem>();

        if (Source is not null)
            items.AddRange(Source.Select(Map).Where(i => i.Date >= inclusiveStart && i.Date <= inclusiveEnd));

        if (Fetch is not null)
            items.AddRange(Fetch(inclusiveStart, inclusiveEnd).Select(Map));

        if (FetchAsync is not null)
        {
            var result = await FetchAsync(inclusiveStart, inclusiveEnd, _cancellationTokenSource.Token);
            items.AddRange(result.Select(Map));
        }

        return items;
    }

    private ScheduleSourceItem Map(T item) => new 
    (
        item!,
        ChildContent(item),
        DateOnly.FromDateTime(DateTime(item).DateTime),
        TimeOnly.FromDateTime(DateTime(item).DateTime),
        Duration(item) ?? TimeSpan.FromHours(1),
        AllDay(item)
    );
}