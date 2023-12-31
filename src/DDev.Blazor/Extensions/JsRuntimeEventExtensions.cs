﻿namespace DDev.Blazor.Extensions;

/// <summary>
/// Extensions method for the <see cref="IJSRuntime"/> to subscribe to document events.
/// </summary>
public static class JsRuntimeEventExtensions
{
    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent(this IJSRuntime js, string eventName, Action handler)
    {
        return OnEvent(js, eventName, null, handler);
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent<T>(this IJSRuntime js, string eventName, Action<T> handler)
    {
        return OnEvent(js, eventName, null, handler);
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent(this IJSRuntime js, string eventName, Func<Task> handler)
    {
        return OnEvent(js, eventName, null, handler);
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent<T>(this IJSRuntime js, string eventName, Func<T, Task> handler)
    {
        return OnEvent(js, eventName, null, handler);
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent(this IJSRuntime js, string eventName, string? reference, Action handler)
    {
        return OnEvent(js, eventName, reference, handler.ToAsync());
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent<T>(this IJSRuntime js, string eventName, string? reference, Action<T> handler)
    {
        return OnEvent(js, eventName, reference, handler.ToAsync());
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static Task<IDisposable> OnEvent(this IJSRuntime js, string eventName, string? reference, Func<Task> handler)
    {
        return OnEvent<object>(js, eventName, reference, _ => handler());
    }

    /// <summary>
    /// Invoke <paramref name="handler"/> every time the document raises an event with name <paramref name="eventName"/>.
    /// </summary>
    public static async Task<IDisposable> OnEvent<T>(this IJSRuntime js, string eventName, string? reference, Func<T, Task> handler)
    {
        var subscription = new JsEventSubscription<T>(handler);

        var listener = await js.InvokeDDevAsync<IJSObjectReference>("events", "createListener", eventName, reference);

        await subscription.SetListener(listener);

        return subscription;
    }

    private class JsEventSubscription<T> : IDisposable
    {
        private readonly Func<T, Task> _handler;
        private IDisposable? _listener;

        public DotNetObjectReference<JsEventSubscription<T>> Reference { get; }

        public JsEventSubscription(Func<T, Task> handler)
        {
            _handler = handler;
            Reference = DotNetObjectReference.Create(this);
        }

        public async Task SetListener(IJSObjectReference listener)
        {
            _listener?.Dispose();
            _listener = null;

            if (listener is not null)
            {
                await listener.InvokeVoidAsync("setDotNetReference", Reference);
                _listener = listener.ToDisposable();
            }
        }

        [JSInvokable("onNextAsync")]
        public async Task OnNextAsync(T args)
        {
            await (_handler?.Invoke(args) ?? Task.CompletedTask);
        }

        public void Dispose()
        {
            _listener?.Dispose();
            Reference.Dispose();
        }
    }
}
