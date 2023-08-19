namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Can portal its content to <see cref="PortalOutlet"/> with a matching <see cref="Name"/>.
/// </summary>
public sealed class PortalSource : ComponentBase, IDisposable
{
    /// <summary>
    /// Name of the portal. Must match a <see cref="PortalOutlet.Name"/> excactly.
    /// </summary>
    [Parameter, EditorRequired] public string Name { get; set; } = "";

    /// <summary>
    /// Content to portal to a different part of the page.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Id used to efficiently render source in outlet.
    /// </summary>
    internal int IdInOutlet { get; set; }

    private string _initialName = "";
    private PortalOutlet? _outlet;

    /// <summary>
    /// Moves the source to the end of the outlets sources.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the source is not attached to a outlet.</exception>
    public void MoveToEnd()
    {
        if (_outlet is null)
            throw new InvalidOperationException($"{nameof(PortalSource)} is not attached to a  {nameof(PortalOutlet)}.");

        _outlet.AddPortal(this);
    }

    protected override void OnInitialized()
    {
        _initialName = Name;
        _outlet = PortalOutlet.Outlets.GetValueOrDefault(_initialName);

        if (_outlet is null)
            throw new InvalidOperationException($"A {nameof(PortalOutlet)} with name {Name} does not exist.");

        _outlet.AddPortal(this);
    }

    protected override void OnParametersSet()
    {
        if (string.Equals(_initialName, Name) is false)
            throw new NotSupportedException($"The {nameof(Name)} parameter on {nameof(PortalSource)} cannot be changed after initialized.");
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        _outlet?.StateHasChangedInSource();
    }

    public void Dispose()
    {
        _outlet?.RemovePortal(this);
    }
}