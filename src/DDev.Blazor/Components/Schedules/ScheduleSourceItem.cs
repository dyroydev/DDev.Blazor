namespace DDev.Blazor.Components.Schedules;

internal record ScheduleSourceItem(object Item, RenderFragment Fragment, DateOnly Date, TimeOnly Time, TimeSpan Duration, bool IsAllDay);