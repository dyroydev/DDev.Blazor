namespace DDev.Blazor.Extensions;

public static class JsRuntimeExtensions
{
    internal static async Task<IJSObjectReference> OpenDDevJsModule(this IJSRuntime runtime, string name)
    {
        return await runtime.InvokeAsync<IJSObjectReference>("import", $"./_content/DDev.Blazor/{name}.js");
    }

    internal static async Task InvokeDDevAsync(this IJSRuntime runtime, string moduleName, string method, params object?[] parameters)
    {
        await using var focusModule = await runtime.OpenDDevJsModule(moduleName);
        await focusModule.InvokeVoidAsync(method, parameters);
    }

    internal static async Task<T> InvokeDDevAsync<T>(this IJSRuntime runtime, string moduleName, string method, params object?[] parameters)
    {
        await using var focusModule = await runtime.OpenDDevJsModule(moduleName);
        return await focusModule.InvokeAsync<T>(method, parameters);
    }

    public static async Task FocusAsync(this IJSRuntime runtime, string? reference) => await InvokeDDevAsync(runtime, "focus", "setFocus", reference);
}