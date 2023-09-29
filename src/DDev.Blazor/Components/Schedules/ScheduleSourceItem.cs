using System.Diagnostics;

namespace DDev.Blazor.Components.Schedules;

[DebuggerDisplay($"{{{nameof(Date)}}} {{{nameof(Time)}}} {{{nameof(Item)}}}")]
internal record ScheduleSourceItem(object Item, RenderFragment Fragment, DateOnly Date, TimeOnly Time, TimeSpan Duration, bool IsAllDay);