namespace DDev.Blazor.Components.Schedules;

internal interface IScheduleSource
{
    Task<IEnumerable<ScheduleSourceItem>> GetBetween(DateOnly inclusiveStart, DateOnly inclusiveEnd);
}
