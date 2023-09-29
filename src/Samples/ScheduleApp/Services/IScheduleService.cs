using ScheduleApp.Models;
using System.Diagnostics;

namespace ScheduleApp.Services;

public interface IScheduleService
{
    Task<List<ScheduleVm>> GetSchedulesAsync(CancellationToken cancellationToken = default);

    Task PutScheduleAsync(ScheduleVm schedule, CancellationToken cancellationToken = default);
    Task DeleteScheduleAsync(ScheduleVm schedule, CancellationToken cancellationToken = default);
}

[DebuggerDisplay($"{{{nameof(Id)}}} - {{{nameof(Name)}}}")]
public class EventVm
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Color { get; set; }

    public TimeSpan? Duration { get; set; }

    public Repeater Repeater { get; set; } = Repeater.Never;
}

public class ScheduleVm
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string? Color { get; set; }

    public List<EventVm> Events { get; set; } = new List<EventVm>();
}
