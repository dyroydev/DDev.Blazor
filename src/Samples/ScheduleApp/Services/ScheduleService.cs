using ScheduleApp.Models;

namespace ScheduleApp.Services;

public class ScheduleService : IScheduleService
{
    private readonly IStorage _storage;

    public ScheduleService(IStorage storage)
    {
        _storage = storage;
    }

    public async Task<List<ScheduleVm>> GetSchedulesAsync(CancellationToken cancellationToken = default)
    {
        return await _storage.GetAllAsync<ScheduleVm>();
    }

    public async Task PutScheduleAsync(ScheduleVm schedule, CancellationToken cancellationToken = default)
    {
        await _storage.PutAsync(schedule.Id.ToString(), schedule, cancellationToken);
    }

    public async Task DeleteScheduleAsync(ScheduleVm schedule, CancellationToken cancellationToken = default)
    {
        await _storage.DeleteAsync<ScheduleVm>(schedule.Id.ToString(), cancellationToken);
    }
}