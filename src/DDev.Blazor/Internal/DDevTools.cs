namespace DDev.Blazor.Internal;

/// <summary>
/// A collection of internal tools used by the library.
/// </summary>
internal class DDevTools
{
    private readonly HashSet<object> _tools = new ();

    /// <summary>
    /// Register a new tool.
    /// </summary>
    /// <param name="tool"></param>
    public void Register(object tool) => _tools.Add(tool);

    /// <summary>
    /// Get the first tool of a given type.
    /// </summary>
    /// <typeparam name="T">Type of tool.</typeparam>
    /// <exception cref="InvalidOperationException">If no tool is registered of the given type.</exception>
    public T Get<T>() => _tools.OfType<T>().FirstOrDefault() ?? throw new InvalidOperationException($"No tool of type {typeof(T).Name} has been registered.");
}