using Microsoft.AspNetCore.Components;
using ScheduleApp.Models;
using ScheduleApp.Services;

namespace ScheduleApp.Pages;

public partial class Index
{
    [Inject] private IScheduleService ScheduleService { get; set; } = null!;
    [Inject] private IMessageBox MessageBox { get; set; } = null!;

    private DateOnly _startOfWeek;
    private DateOnly _startOfMonth;
    private ViewMode _viewMode = ViewMode.Month;

    private HashSet<Guid> _selectedScheduleIds = new();
    private List<ScheduleVm> _schedules = new();

    private TemplatePopupMenu<ScheduleVm>? _popupMenu;
    private SideSheet? _sidebar;
    private EditScheduleDialog? _editScheduleDialog;
    private EditEventDialog? _editEventDialog;

    protected override async Task OnInitializedAsync()
    {
        _startOfMonth = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
        _startOfWeek = DateOnly.FromDateTime(DateTime.Now);
        while (_startOfWeek.DayOfWeek != DayOfWeek.Monday)
            _startOfWeek = _startOfWeek.AddDays(-1);

        _schedules = await ScheduleService.GetSchedulesAsync();
        _selectedScheduleIds.Add(_schedules.FirstOrDefault()!.Id);
    }

    private string GetTitle()
    {
        var schedules = GetSelectedSchedules();

        if (schedules.Count == 0)
            return "Schedules";

        if (schedules.Count == 1)
            return schedules.First().Name;

        return $"{string.Join(", ", schedules.Take(schedules.Count - 1).Select(s => s.Name))} and {schedules.Last().Name}";
    }

    private string GetColor((ScheduleVm Schedule, EventVm Event, DateTimeOffset Time) context)
    {
        var scheduleColor = context.Schedule.Color ?? CssColor.RandomNeutral(context.Schedule.Id);
        return GetSelectedSchedules().Count > 1
            ? scheduleColor
            : context.Event.Color ?? scheduleColor;
    }

    private IEnumerable<(ScheduleVm Schedule, EventVm Event, DateTimeOffset Time)> GetEventsBetween(DateOnly from, DateOnly to)
    {
        var result = GetSelectedSchedules().SelectMany(
            s => s.Events.SelectMany(
            e => e.Repeater.GetBetween(from.ToDateTime(TimeOnly.MinValue), to.ToDateTime(TimeOnly.MaxValue)).Select(
            t => (s, e, t)))).ToList();
        return result;
    }

    private List<ScheduleVm> GetSelectedSchedules()
    {
        return _schedules.FindAll(s => _selectedScheduleIds.Contains(s.Id));
    }

    private async Task DeleteSchedule(ScheduleVm schedule)
    {
        if (await MessageBox.ConfirmAsync(
            title: $"Delete {schedule.Name}",
            statement: $"Do you really want to delete the schedule \"{schedule.Name}\"?",
            confirm: "Yes, delete", dismiss: "No, go back"))
        {
            await ScheduleService.DeleteScheduleAsync(schedule);
            _schedules = await ScheduleService.GetSchedulesAsync();
        }
    }

    private async Task CreateNewSchedule()
    {
        if (await _editScheduleDialog!.CreateNewAsync())
        {
            _schedules = await ScheduleService.GetSchedulesAsync();
        }
    }

    private async Task EditSchedule(ScheduleVm schedule)
    {
        if (await _editScheduleDialog!.EditAsync(schedule))
        {
            _schedules = await ScheduleService.GetSchedulesAsync();
        }
    }

    private void ClickSchedule(ScheduleVm schedule)
    {
        if (_selectedScheduleIds.Contains(schedule.Id))
            _selectedScheduleIds.Remove(schedule.Id);
        else
            _selectedScheduleIds.Add(schedule.Id);
    }

    private async Task ClickEvent(EventVm eventVm)
    {
        await _editEventDialog!.EditAsync(eventVm);
    }

    private void ClickOpenMenu()
    {
        _sidebar!.Show();
    }
}
