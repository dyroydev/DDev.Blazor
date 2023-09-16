namespace DDev.Blazor.Internal;

internal class DDevTools
{
    private readonly HashSet<object> _tools = new ();

    public void Register(object tool) => _tools.Add(tool);

    public T Get<T>() => _tools.OfType<T>().FirstOrDefault() ?? throw new InvalidOperationException($"No tool of type {typeof(T).Name} has been registered.");
}