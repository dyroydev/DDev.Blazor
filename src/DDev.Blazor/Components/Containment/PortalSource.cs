using DDev.Blazor.Internal;
using System.Diagnostics;

namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Can portal its content to <see cref="PortalOutlet"/> with a matching <see cref="Name"/>.
/// </summary>
[DebuggerDisplay($"{nameof(PortalSource)} {{{nameof(Name)}}}, {nameof(IsConnected)} = {{{nameof(IsConnected)}}}")]
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

    [Inject] private PortalRegistry Portals { get; set; } = null!;

    /// <summary>
    /// If <see langword="true"/>, the portal is connected to a <see cref="PortalOutlet"/>.
    /// </summary>
    public bool IsConnected => Outlet is not null;

    /// <summary>
    /// Id used to efficiently render source in outlet.
    /// </summary>
    internal int IdInOutlet { get; set; }

    /// <summary>
    /// Outlet provided by the internal portal registry.
    /// </summary>
    internal PortalOutlet? Outlet { get; set; }

    private string _initialName = "";

    /// <summary>
    /// Notifies the component that its state has changed. This will cause to component to render when applicable.
    /// </summary>
    internal void NotifyStateHasChanged()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Moves the source to the end of the outlets sources.
    /// </summary>
    public void MoveToEnd()
    {
        Portals.Add(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        _initialName = Name;
        Portals.Add(this);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (string.Equals(_initialName, Name) is false)
            throw new NotSupportedException($"The {nameof(Name)} parameter on {nameof(PortalSource)} cannot be changed after initialized.");
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        Outlet?.NotifyStateHasChanged();
    }

    void IDisposable.Dispose()
    {
        Portals.Remove(this);
    }
}