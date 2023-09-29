using DDev.Blazor.Internal;
using System.Diagnostics;

namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Can render content from one or more <see cref="PortalSource"/> components with a matching <see cref="Name"/>.
/// </summary>
[DebuggerDisplay($"{nameof(PortalOutlet)} {{{nameof(Name)}}}")]
public sealed class PortalOutlet : ComponentBase, IDisposable
{
    /// <summary>
    /// Name of portal.
    /// </summary>
    [Parameter, EditorRequired] public string Name { get; set; } = "";

    [Inject] private PortalRegistry Portals { get; set; } = null!;

    internal List<PortalSource> Sources { get; } = new();

    private string _initialName = "";

    /// <summary>
    /// Notifies the outlet that some source has changed its content and the outlet must render.
    /// </summary>
    internal void NotifyStateHasChanged()
    {
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        foreach (var portal in Sources.OrderBy(s => s.IdInOutlet))
        {
            builder.AddContent(portal.IdInOutlet, portal.ChildContent);
        }
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
            throw new NotSupportedException($"The {nameof(Name)} parameter on {nameof(PortalOutlet)} cannot be changed after initialized.");
    }

    void IDisposable.Dispose()
    {
        Portals.Remove(this);
    }
}