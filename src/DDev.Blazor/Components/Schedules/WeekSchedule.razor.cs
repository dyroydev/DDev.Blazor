namespace DDev.Blazor.Components.Schedules;

/// <summary>
/// A schedule rendering one month at a time.
/// </summary>
public partial class WeekSchedule : IScheduleSourceContainer
{
    /// <summary>
    /// A date in the first week.
    /// </summary>
    [Parameter] public DateOnly StartWeek { get; set; }

    /// <summary>
    /// Number of
    /// </summary>
    [Parameter] public int WeekCount { get; set; } = 5;

    /// <summary>
    /// Should be a collection of <see cref="ScheduleSource{T}"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback invoked when the user clicks a date.
    /// </summary>
    [Parameter] public EventCallback<DateOnly> OnClickDate { get; set; }

    private readonly HashSet<IScheduleSource> _sources = new();
    private Dictionary<DateOnly, List<ScheduleSourceItem>> _items = new();
    private DateOnly _firstDate;
    private DateOnly _today;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        _today = DateOnly.FromDateTime(DateTime.Now);
        _firstDate = StartWeek.GetStartOfWeek();

        await LoadItemsAsync();
    }

    private async Task LoadItemsAsync()
    {
        var loads = _sources.Select(s => s.GetBetween(_firstDate, _firstDate.AddDays(WeekCount * 7)));
        var results = await Task.WhenAll(loads);

        _items = results
            .SelectMany(r => r)
            .OrderBy(i => i.Time)
            .GroupBy(i => i.Date)
            .ToDictionary(group => group.Key, group => group.ToList());
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