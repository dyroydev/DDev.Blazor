using System.Collections.Concurrent;

namespace DDev.Blazor.Services;

internal class KeyBindings : IKeyBindings
{
    private readonly ConcurrentDictionary<int, Func<Task>> _callbacks = new();
    private readonly IJSRuntime _js;
    private readonly object? _reference;
    private readonly DotNetObjectReference<KeyBindings> _this;
    private readonly Lazy<Task> _initialize;
    private readonly Task _jsReady;
    private IJSObjectReference? _bindings;
    private int _callbackId = 0;
    private bool _disposed = new();

    public KeyBindings(IJSRuntime js, Task jsReady, object? reference)
    {
        _js = js;
        _jsReady = jsReady;
        _reference = reference;
        _this = DotNetObjectReference.Create(this);
        _initialize = new Lazy<Task>(InitializeAsync);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        _initialize.Value.ContinueWith(_ =>
        {
            _bindings?.ToDisposable().Dispose();
            _this.Dispose();
        }).Discard("Failed to dispose");
    }

    public IKeyBindings On(string key, ModifierKey modifier, Func<Task> callback)
    {
        AddCallbackAsync(key, modifier, callback).Discard("Failed to register callback");
        return this;
    }

    [JSInvokable]
    public async Task InvokeCallbackAsync(int callbackId)
    {
        var task = _callbacks.GetValueOrDefault(callbackId)?.Invoke();
        if (task is not null && task.IsCompleted is false)
            await task;
    }

    private async Task AddCallbackAsync(string key, ModifierKey modifier, Func<Task> callback)
    {
        await _initialize.Value;

        if (_disposed)
            return;

        var callbackId = Interlocked.Increment(ref _callbackId);
        _callbacks[callbackId] = callback;
        await _bindings!.InvokeVoidAsync("addCallback", callbackId, key, modifier.HasFlag(ModifierKey.Shift), modifier.HasFlag(ModifierKey.Control), modifier.HasFlag(ModifierKey.Alt), modifier.HasFlag(ModifierKey.Meta));
    }

    private async Task InitializeAsync()
    {
        if (_disposed)
            return;

        await _jsReady;

        _bindings = await _js.InvokeDDevAsync<IJSObjectReference>("keybinds", "create", _this, _reference);
    }

    public IKeyBindings On(string key, Action callback) => On(key, ModifierKey.None, callback.ToAsync());

    public IKeyBindings On(string key, Func<Task> callback) => On(key, ModifierKey.None, callback);

    public IKeyBindings On(string key, ModifierKey modifier, Action callback) => On(key, modifier, callback.ToAsync());
}