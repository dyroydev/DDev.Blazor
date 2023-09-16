using DDev.Blazor.Internal;

namespace DDev.Blazor.Components;

/// <summary>
/// Main entry point for DDev Blazor. Must be added to top of &lt;App/&gt; component.
/// </summary>
public partial class DDevBlazor
{
    [Inject] DDevTools Tools { get; set; } = null!;

    /// <summary>
    /// Renders an internal component.
    /// </summary>
    private RenderFragment AddTool<T>() where T : IComponent
    {
        return builder =>
        {
            builder.OpenComponent<T>(0);
            builder.AddComponentReferenceCapture(1, tool => Tools.Register(tool));
            builder.CloseComponent();
        };
    }
}