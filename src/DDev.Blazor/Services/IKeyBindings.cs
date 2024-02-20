namespace DDev.Blazor.Services;

/// <summary>
/// Represents a series of key-bindings connected to a element or the document.
/// </summary>
public interface IKeyBindings : IDisposable
{
    /// <summary>
    /// Invoked callback when a keyboard event is raised with the given <paramref name="key"/>.
    /// </summary>
    IKeyBindings On(string key, Action callback);

    /// <summary>
    /// Invoked callback when a keyboard event is raised with the given <paramref name="key"/>.
    /// </summary>
    IKeyBindings On(string key, Func<Task> callback);

    /// <summary>
    /// Invoked callback when a keyboard event is raised with the given <paramref name="key"/> and <paramref name="modifier"/>.
    /// </summary>
    IKeyBindings On(string key, ModifierKey modifier, Action callback);

    /// <summary>
    /// Invoked callback when a keyboard event is raised with the given <paramref name="key"/> and <paramref name="modifier"/>.
    /// </summary>
    IKeyBindings On(string key, ModifierKey modifier, Func<Task> callback);
}
