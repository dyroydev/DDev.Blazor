using Microsoft.AspNetCore.Components;
using ScheduleApp.Models;
using ScheduleApp.Services;

namespace ScheduleApp.Components;

public partial class EditScheduleDialog
{
    [Inject] private IScheduleService ScheduleService { get; set; } = null!;

    private readonly ScheduleEditModel _model = new();
    private ScheduleVm? _original;
    private Dialog? _dialog;

    public async Task<bool> CreateNewAsync()
    {
        return await EditAsync(null);
    }

    public async Task<bool> EditAsync(ScheduleVm? schedule)
    {
        if (_dialog is null)
            return false;

        _original = schedule;

        schedule ??= new ScheduleVm
        {
            Id = Guid.NewGuid(),
            Color = CssColor.RandomNeutral(null)
        };

        _model.Model = schedule;

        var shouldCreateNew = await _dialog.ShowAsync<bool>();
        if (shouldCreateNew is false)
            return false;

        _model.ApplyChanges();
        await ScheduleService.PutScheduleAsync(schedule!);
        return true;
    }
}
