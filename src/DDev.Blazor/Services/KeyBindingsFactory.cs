using DDev.Blazor.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DDev.Blazor.Services;

internal class KeyBindingsFactory(IJSRuntime js, [FromKeyedServices(nameof(DDevBlazor))] TaskCompletionSource jsReady) : IKeyBindingsFactory
{
    public IKeyBindings Create()
    {
        return new KeyBindings(js, jsReady.Task, null);
    }

    public IKeyBindings Create(ElementReference element)
    {
        return new KeyBindings(js, jsReady.Task, element);
    }

    public IKeyBindings Create(string elementReference)
    {
        return new KeyBindings(js, jsReady.Task, elementReference);
    }
}
