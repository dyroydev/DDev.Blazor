using Microsoft.AspNetCore.Components;

namespace DDev.Blazor.Services;

/// <summary>
/// A service that can create key bindings to elements and documents.
/// </summary>
public interface IKeyBindingsFactory
{
    /// <summary>
    /// Creates a new set of key-bindings for the document.
    /// </summary>
    IKeyBindings Create();

    /// <summary>
    /// Creates a new set of key-bindings for the given <paramref name="element"/>.
    /// </summary>
    IKeyBindings Create(ElementReference element);

    /// <summary>
    /// Creates a new set of key-bindings for the element referenced by <paramref name="elementReference"/>.
    /// </summary>
    IKeyBindings Create(string elementReference);
}
