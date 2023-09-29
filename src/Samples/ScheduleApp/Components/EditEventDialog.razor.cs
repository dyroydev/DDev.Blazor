using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ScheduleApp.Models;
using ScheduleApp.Services;

namespace ScheduleApp.Components;

public partial class EditEventDialog
{
    [Inject] private IScheduleService ScheduleService { get; set; } = null!;

    [Inject] private IMessageBox MessageBox { get; set; } = null!;

    private readonly EventEditModel _model = new();
    private List<ScheduleVm> _schedules = new();
    private ScheduleVm? _originalSchedule;
    private EventVm? _original;
    private Dialog? _dialog;

    public async Task CreateNewAsync(DateOnly? date = null, TimeOnly? time = null)
    {
        await ShowAsync(null, null, date, time);
    }

    public async Task EditAsync(ScheduleVm? schedule, EventVm? eventVm)
    {
        await ShowAsync(schedule, eventVm, null, null);
    }

    private async Task ShowAsync(ScheduleVm? schedule, EventVm? eventVm, DateOnly? overrideDate, TimeOnly? overrideTime)
    {
        if (_dialog is null)
            return;

        _schedules = await ScheduleService.GetSchedulesAsync();
        _originalSchedule = _schedules.FirstOrDefault(s => s.Id == schedule?.Id);
        _original = eventVm;

        eventVm ??= new EventVm
        {
            Id = Guid.NewGuid(),
            Color = CssColor.RandomNeutral(null)
        };

        _model.Model = eventVm;
        _model.Schedule = _originalSchedule;

        if (overrideDate.HasValue)
            _model.Model.Repeater.Start = new DateTimeOffset(overrideDate.Value.Year, overrideDate.Value.Month, overrideDate.Value.Day, _model.Model.Repeater.Start.Hour, _model.Model.Repeater.Start.Minute, _model.Model.Repeater.Start.Second, _model.Model.Repeater.Start.Offset);

        if (overrideTime.HasValue)
            _model.Model.Repeater.Start = new DateTimeOffset(_model.Model.Repeater.Start.Year, _model.Model.Repeater.Start.Month, _model.Model.Repeater.Start.Day, overrideTime.Value.Hour, overrideTime.Value.Minute, overrideTime.Value.Second, _model.Model.Repeater.Start.Offset);

        var shouldSave = await _dialog.ShowAsync<bool>();
        if (shouldSave is false)
            return;

        _model.ApplyChanges();

        if (_model.Schedule is null)
        {
            await MessageBox.AlertAsync("Event is missing required field: Schedule.", "Could not save event");
            return;
        }

        _originalSchedule?.Events.RemoveAll(e => e.Id == eventVm.Id);
        _model.Schedule.Events.RemoveAll(e => e.Id == eventVm.Id);
        _model.Schedule.Events.Add(eventVm);

        if (_originalSchedule is not null)
            await ScheduleService.PutScheduleAsync(_originalSchedule);

        await ScheduleService.PutScheduleAsync(_model.Schedule);
    }

    public async Task<bool> DeleteAsync(ScheduleVm schedule, EventVm eventVm)
    {
        _originalSchedule = schedule;
        _original = eventVm;

        var dialog = _dialog?.ShowAsync<bool>() ?? Task.FromResult(false);

        await DeleteAsync();

        return await dialog;
    }

    private async Task DeleteAsync()
    {
        var confirmed = await MessageBox.ConfirmAsync(
            title: $"Delete event",
            statement: $"Are you sure you want to delete event {_original?.Name}",
            confirm: "Delete", dismiss: "Cancel");

        if (confirmed is false)
            return;

        _dialog?.CloseWithResult(false);

        if (_original is null || _originalSchedule is null)
            return;

        _originalSchedule.Events.RemoveAll(e => e.Id == _original.Id);
        await ScheduleService.PutScheduleAsync(_originalSchedule);
    }
}
