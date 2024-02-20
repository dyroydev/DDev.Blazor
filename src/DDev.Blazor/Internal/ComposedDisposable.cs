namespace DDev.Blazor.Internal;

/// <summary>
/// Composed disposable objects.
/// </summary>
internal class ComposedDisposable : IDisposable, IAsyncDisposable
{
    private readonly List<IDisposable> _disposables = [];
    private readonly List<IAsyncDisposable> _asyncDisposables = [];

    /// <summary>
    /// Adds <paramref name="obj"/> if it implements <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.
    /// </summary>
    public void AddIfDisposable(object? obj)
    {
        if (obj is IAsyncDisposable asyncDisposable)
            _asyncDisposables.Add(asyncDisposable);

        else if (obj is IDisposable disposable)
            _disposables.Add(disposable);
    }

    /// <summary>
    /// Disposes all added disposables.
    /// </summary>
    /// <remarks>
    /// Once disposed, this objects is cleared and may be reused.<br/>
    /// Might block the calling thread until all disposables are disposed.
    /// </remarks>
    public void Dispose()
    {
        var result = DisposeAsync();

        if (result.IsCompleted && result.IsCompletedSuccessfully)
            return;

        result.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Disposes all added disposables.
    /// </summary>
    /// <remarks>Once disposed, this objects is cleared and may be reused.</remarks>
    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();

        foreach (var disposable in _asyncDisposables)
            await disposable.DisposeAsync();

        _disposables.Clear();
        _asyncDisposables.Clear();
    }
}
