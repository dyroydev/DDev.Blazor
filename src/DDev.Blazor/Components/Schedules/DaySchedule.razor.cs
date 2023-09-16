namespace DDev.Blazor.Components.Schedules;

/// <summary>
/// Renders items in a schedule for a given amount of days.
/// </summary>
public partial class DaySchedule : IScheduleSourceContainer, IDisposable
{
    /// <summary>
    /// Should contain only components of type <see cref="ScheduleSource{T}"/>.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The first day in the schedule.
    /// </summary>
    [Parameter] public DateOnly StartDay { get; set; }

    /// <summary>
    /// Number of days in the schedule. Must be greater than or equal to <c>1</c>.
    /// </summary>
    /// <remarks>Default is <c>1</c>.</remarks>
    [Parameter] public int DayCount { get; set; } = 1;

    /// <summary>
    /// Length of one hour in view-space.
    /// </summary>
    /// <remarks>Default is <c>80px</c>.</remarks>
    [Parameter] public string HourSize { get; set; } = "80px";

    /// <summary>
    /// Time the schedule starts.
    /// </summary>
    [Parameter] public TimeOnly StartTime { get; set; } = TimeOnly.MinValue;

    /// <summary>
    /// Time the schedule end.
    /// </summary>
    [Parameter] public TimeOnly EndTime { get; set; } = TimeOnly.MaxValue;

    /// <summary>
    /// Very short informational text rendered in top-left corner of schedule.
    /// </summary>
    [Parameter] public string? Info { get; set; }

    /// <summary>
    /// Very short header for informational text rendered in top-left corner of schedule.
    /// </summary>
    [Parameter] public string? InfoHeader { get; set; }

    /// <summary>
    /// Callback invoked when the user clicks a time.
    /// </summary>
    [Parameter] public EventCallback<(DateOnly Date, TimeOnly Time)> OnClickTime { get; set; }

    private readonly HashSet<IScheduleSource> _sources = new();
    private Dictionary<DateOnly, List<ScheduleSourceItem>> _items = new();
    private Dictionary<ScheduleSourceItem, (int count, int index)> _splits = new();
    private DateOnly[] _days = Array.Empty<DateOnly>();
    private DateOnly _today;
    private Timer? _timer;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (DayCount < 1)
            throw new ArgumentException("Days must be greater than or equal to 1.", nameof(DayCount));

        if (StartTime > EndTime)
            throw new ArgumentException("StartTime must be less than or equal to EndTime.", nameof(StartTime));

        _today = DateOnly.FromDateTime(DateTime.Now);
        _days = new DateOnly[DayCount];
        for (var i = 0; i < _days.Length; i++)
            _days[i] = StartDay.AddDays(i);

        await LoadItemsAsync();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        var now = DateTime.Now;
        var oneMinute = TimeSpan.FromMinutes(1);
        var nextMinute = new TimeSpan(now.Hour, now.Minute, 0).Add(oneMinute);
        var timeToNextMinute = nextMinute - now.TimeOfDay;

        _timer = new Timer(HandleTick, state:null, timeToNextMinute, oneMinute);
    }

    private static string GetHours(TimeSpan? time) => time?.TotalHours.ToString().Replace(',', '.') ?? "";

    private static string GetHours(TimeOnly? time) => GetHours(time?.ToTimeSpan());

    private void HandleTick(object? timerState)
    {
        _today = DateOnly.FromDateTime(DateTime.Now);
        InvokeAsync(StateHasChanged).Discard("Tick failed");
        InvokeAsync(LoadItemsAsync).Discard("Load schedule items failed");
    }

    private async Task LoadItemsAsync()
    {
        var loads = _sources.Select(s => s.GetBetween(StartDay, StartDay.AddDays(DayCount)));
        var results = await Task.WhenAll(loads);

        _items = results
            .SelectMany(r => r)
            .OrderBy(i => i.Time)
            .GroupBy(i => i.Date)
            .ToDictionary(group => group.Key, group => group.ToList());

        RecalculateSplits();

        StateHasChanged();
    }

    private void RecalculateSplits()
    {
        _splits.Clear();

        // Assumes _items has been sorted.
        foreach (var (date, items) in _items)
        {
            var openItems = new List<ScheduleSourceItem>();
            foreach (var item in items)
            {
                if (openItems.Count == 0)
                {
                    openItems.Add(item);
                    _splits[item] = (1, 0);
                    continue;
                }

                // Removed ended items
                openItems.RemoveAll(i => item.Time >= i.Time.Add(i.Duration));

                openItems.Add(item);

                // Recalculate splits for open items.
                var i = 0;
                foreach (var openItem in openItems)
                    _splits[openItem] = (openItems.Count, i++);
            }
        }
    }

    void IDisposable.Dispose()
    {
        _timer?.Dispose();
    }

    void IScheduleSourceContainer.AddScheduleSource(IScheduleSource source)
    {
        if (_sources.Add(source))
        {
            StateHasChanged();
            InvokeAsync(LoadItemsAsync).Discard("Load schedule items failed");
        }
    }

    void IScheduleSourceContainer.RemoveScheduleSource(IScheduleSource source)
    {
        _sources.Remove(source);
        StateHasChanged();
    }
}