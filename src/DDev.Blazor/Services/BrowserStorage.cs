namespace DDev.Blazor.Services;

internal sealed class LocalStorage(IJSRuntime js) : BrowserStorage(js, "localStorage"), ILocalStorage
{
}

internal sealed class SessionStorage(IJSRuntime js) : BrowserStorage(js, "sessionStorage"), ISessionStorage
{
}

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
