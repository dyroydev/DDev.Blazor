using System.Collections.Concurrent;

namespace DDev.Blazor.Services;

/// <summary>
/// A service that can create keybindings to elements and documents.
/// </summary>
public interface IKeyBindingsFactory
{
    /// <summary>
    /// Creates a new set of key-bindings for the document.
    /// </summary>
    IKeyBindings ForDocument();

    /// <summary>
    /// Creates a new set of key-bindings for the given <paramref name="element"/>.
    /// </summary>
    IKeyBindings ForElement(ElementReference element);

    /// <summary>
    /// Creates a new set of key-bindings for the element referenced by <paramref name="elementReference"/>.
    /// </summary>
    IKeyBindings ForElement(string elementReference);
}

/// <summary>
/// Represents a series of key-bindings connected to a element or the document.
/// </summary>
public interface IKeyBindings : IDisposable
{
    /// <summary>
    /// Invoked callback when a keybaord event is raised with the given <paramref name="key"/>.
    /// </summary>
    IKeyBindings On(string key, Action callback) => On(key, callback.ToAsync());

    /// <summary>
    /// Invoked callback when a keybaord event is raised with the given <paramref name="key"/>.
    /// </summary>
    IKeyBindings On(string key, Func<Task> callback) => On(key, ModifierKey.None, callback);

    /// <summary>
    /// Invoked callback when a keybaord event is raised with the given <paramref name="key"/> and <paramref name="modifier"/>.
    /// </summary>
    IKeyBindings On(string key, ModifierKey modifier, Action callback) => On(key, modifier, callback.ToAsync());

    /// <summary>
    /// Invoked callback when a keybaord event is raised with the given <paramref name="key"/> and <paramref name="modifier"/>.
    /// </summary>
    IKeyBindings On(string key, ModifierKey modifier, Func<Task> callback);
}

/// <summary>
/// A modifier-key on the keyboard.
/// </summary>
[Flags]
public enum ModifierKey
{
    /// <summary>
    /// No control keys.
    /// </summary>
    None = 0,
    /// <summary>
    /// Alt-key on windows, Option-key on mac.
    /// </summary>
    Alt = 1,
    /// <summary>
    /// Control-key on windows, Command-key on mac.
    /// </summary>
    Control = 2,
    /// <summary>
    /// Shift-key.
    /// </summary>
    Shift = 4,
    /// <summary>
    /// Windows-key on windows, Command-key on mac.
    /// </summary>
    Meta = 8,
}

internal class KeyBindingsFactoryInternal : IKeyBindingsFactory
{
    private readonly IJSRuntime _js;

    public KeyBindingsFactoryInternal(IJSRuntime js)
    {
        _js = js;
    }

    public IKeyBindings ForDocument()
    {
        return new KeyBindingsInternal(_js, null);
    }

    public IKeyBindings ForElement(ElementReference element)
    {
        return new KeyBindingsInternal(_js, element);
    }

    public IKeyBindings ForElement(string elementReference)
    {
        return new KeyBindingsInternal(_js, elementReference);
    }
}

internal class KeyBindingsInternal : IKeyBindings
{
    private readonly ConcurrentDictionary<int, Func<Task>> _callbacks = new();
    private readonly IJSRuntime _js;
    private readonly object? _reference;
    private readonly DotNetObjectReference<KeyBindingsInternal> _this;
    private readonly Lazy<Task> _initialize;
    private IJSObjectReference? _bindings;
    private int _callbackId = 0;
    private bool _disposed = new();

    public KeyBindingsInternal(IJSRuntime js, object? reference)
    {
        _js = js;
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

        _bindings = await _js.InvokeDDevAsync<IJSObjectReference>("keybinds", "create", _this, _reference);
    }
}