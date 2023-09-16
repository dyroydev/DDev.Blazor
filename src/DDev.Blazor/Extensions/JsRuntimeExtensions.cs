namespace DDev.Blazor.Extensions;

/// <summary>
/// Extensions for <see cref="IJSRuntime"/> to help with common tasks.
/// </summary>
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

    /// <summary>
    /// Gives focus to the element referred to by <paramref name="reference"/>.
    /// </summary>
    public static async Task FocusAsync(this IJSRuntime runtime, string? reference) => await InvokeDDevAsync(runtime, "focus", "setFocus", reference);

    /// <summary>
    /// Return <see langword="true"/> if the element referred to by <paramref name="reference"/> has or contains the element that has focus.
    /// </summary>
    public static async Task<bool> HasFocusAsync(this IJSRuntime runtime, string? reference) => await InvokeDDevAsync<bool>(runtime, "focus", "hasFocus", reference);

    /// <summary>
    /// Return <see langword="true"/> if the <paramref name="element"/> has or contains the element that has focus.
    /// </summary>
    public static async Task<bool> HasFocusAsync(this IJSRuntime runtime, ElementReference? element) => await InvokeDDevAsync<bool>(runtime, "focus", "hasFocus", element);
}