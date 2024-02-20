namespace DDev.Blazor.Services;

/// <summary>
/// Key-value store using the browser's local storage. (<a href="https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage">mdn</a>)
/// </summary>
public interface ILocalStorage : IBrowserStorage
{
}

/// <summary>
/// Key-value store using the browser's session storage. (<a href="https://developer.mozilla.org/en-US/docs/Web/API/Window/sessionStorage">mdn</a>)
/// </summary>
public interface ISessionStorage : IBrowserStorage
{
}

/// <summary>
/// Key-value store using the browser's Web Storage API. (<a href="https://developer.mozilla.org/en-US/docs/Web/API/Storage">mdn</a>)
/// </summary>
public interface IBrowserStorage
{
    /// <summary>
    /// Returns the number of values in the storage.
    /// </summary>
    Task<int> Count(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the value associated with the specified key. <see langword="null"/> if the key does not exist.
    /// </summary>
    Task<string?> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the value associated with the specified key.
    /// </summary>
    Task SetAsync(string key, string? value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the value with the specified key.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes all values from the storage.
    /// </summary>
    Task ClearAsync(CancellationToken cancellationToken = default);
} 