namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Can render content from one or more <see cref="PortalSource"/> components with a matching <see cref="Name"/>.
/// </summary>
public sealed class PortalOutlet : ComponentBase, IDisposable
{
    /// <summary>
    /// Name of portal.
    /// </summary>
    [Parameter, EditorRequired] public string Name { get; set; } = "";

    /// <summary>
    /// Internal dictionary of <see cref="PortalOutlet"/> by name.
    /// </summary>
    internal static Dictionary<string, PortalOutlet> Outlets { get; } = new();

    private readonly List<PortalSource> _portals = new();
    private int _nextId;
    private string _initialName = "";

    /// <summary>
    /// Adds <paramref name="portal"/> to the outlet. The portal is given a new <see cref="PortalSource.IdInOutlet"/>.
    /// </summary>
    /// <remarks>If the <paramref name="portal"/> already exists in the outlet, it is moved to the bottom.</remarks>
    internal void AddPortal(PortalSource portal)
    {
        if (_portals.Contains(portal))
            _portals.Remove(portal);

        portal.IdInOutlet = _nextId++;
        _portals.Add(portal);
        StateHasChanged();
    }

    /// <summary>
    /// Notifies the outlet that some source has changed its content and the outlet must render.
    /// </summary>
    internal void StateHasChangedInSource()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Removes <paramref name="portal"/> from the outlet.
    /// </summary>
    internal void RemovePortal(PortalSource portal)
    {
        _portals.Remove(portal);
        StateHasChanged();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        foreach (var portal in _portals)
        {
            builder.AddContent(portal.IdInOutlet, portal.ChildContent);
        }
    }

    protected override void OnInitialized()
    {
        _initialName = Name;
        if (Outlets.ContainsKey(_initialName))
            throw new InvalidOperationException($"Only one {nameof(PortalOutlet)} can have the name {Name}");

        Outlets.Add(_initialName, this);
    }

    protected override void OnParametersSet()
    {
        if (string.Equals(_initialName, Name) is false)
            throw new NotSupportedException($"The {nameof(Name)} parameter on {nameof(PortalOutlet)} cannot be changed after initialized.");
    }

    public void Dispose()
    {
        Outlets.Remove(_initialName, out _);
    }
}