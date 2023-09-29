namespace DDev.Blazor.Internal;

/// <summary>
/// A stack of backdrops to keep track of which is on top.
/// </summary>
internal class BackdropStack : IDisposable
{
    private readonly List<Backdrop> _backdrops = new();
    private readonly SemaphoreSlim _semaphore = new(1);

    /// <summary>
    /// Pushes <paramref name="backdrop"/> to the top of the backdrop stack.
    /// </summary>
    public async Task PushAsync(Backdrop backdrop)
    {
        await _semaphore.WaitAsync();

        try
        {
            _backdrops.Remove(backdrop);
            _backdrops.Add(backdrop);

            if (_backdrops.Count > 1)
                await _backdrops[^2].NotifyStackTopAsync(false);

            await backdrop.NotifyStackTopAsync(true);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Removes <paramref name="backdrop"/> from anywhere on the stack.
    /// </summary>
    public async Task Remove(Backdrop backdrop)
    {
        await _semaphore.WaitAsync();

        try
        {
            _backdrops.Remove(backdrop);

            await backdrop.NotifyStackTopAsync(false);

            if (_backdrops.Count > 0)
                await _backdrops[^1].NotifyStackTopAsync(true);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    void IDisposable.Dispose()
    {
        _semaphore.Dispose();
    }
}
