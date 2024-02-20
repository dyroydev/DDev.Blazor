namespace DDev.Blazor.Services;

/// <summary>
/// <see cref="BrowserStorage"/> extension for local storage.
/// </summary>
internal sealed class LocalStorage(IJSRuntime js) : BrowserStorage(js, "localStorage"), ILocalStorage;

/// <summary>
/// <see cref="BrowserStorage"/> extension for session storage.
/// </summary>
internal sealed class SessionStorage(IJSRuntime js) : BrowserStorage(js, "sessionStorage"), ISessionStorage;

/// <summary>
/// Wraps the browser's storage API.
/// </summary>
/// <param name="js"></param>
/// <param name="provider">Name of the browser storage property. One of <c>localStorage</c> or <c>sessionStorage</c></param>
internal abstract class BrowserStorage(IJSRuntime js, string provider) : IBrowserStorage
{
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await js.InvokeVoidAsync($"{provider}.clear");
    }

    public async Task<int> Count(CancellationToken cancellationToken = default)
    {
        return await js.InvokeAsync<int>($"{provider}.length");
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await js.InvokeAsync<string?>($"{provider}.getItem", key);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await js.InvokeVoidAsync($"{provider}.removeItem", key);
    }

    public async Task SetAsync(string key, string? value, CancellationToken cancellationToken = default)
    {
        await js.InvokeVoidAsync($"{provider}.setItem", key, value);
    }
}
