using Microsoft.AspNetCore.Components;
using ScheduleApp.Models;
using ScheduleApp.Services;

namespace ScheduleApp.Components;

public partial class EditEventDialog
{
    [Inject] private IScheduleService ScheduleService { get; set; } = null!;

    private readonly EventEditModel _model = new();
    private EventVm? _original;
    private Dialog? _dialog;

    public async Task<EventVm?> CreateNewAsync()
    {
        return await EditAsync(null);
    }

    public async Task<EventVm?> EditAsync(EventVm? eventVm)
    {
        if (_dialog is null)
            return null;

        _original = eventVm;

        eventVm ??= new EventVm
        {
            Id = Guid.NewGuid(),
            Color = CssColor.RandomNeutral(null)
        };

        _model.Model = eventVm;

        var shouldCreateNew = await _dialog.ShowAsync<bool>();
        if (shouldCreateNew is false)
            return null;

        _model.ApplyChanges();
        return _model.Model;
    }
}
