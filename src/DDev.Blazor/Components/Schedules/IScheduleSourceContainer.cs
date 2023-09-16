namespace DDev.Blazor.Components.Schedules;

internal interface IScheduleSourceContainer
{
    void AddScheduleSource(IScheduleSource source);

    void RemoveScheduleSource(IScheduleSource source);
}
