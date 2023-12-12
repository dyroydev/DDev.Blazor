using Microsoft.Extensions.DependencyInjection;

namespace DDev.Blazor.Components;

/// <summary>
/// Main entry point for DDev Blazor. Must be added to top of &lt;App/&gt; component.
/// </summary>
public partial class DDevBlazor
{
    [Inject] IServiceProvider Services { get; set; } = null!;

    /// <summary>
    /// Renders an internal component.
    /// </summary>
    private RenderFragment AddTool<T>() where T : IComponent
    {
        var taskSource = Services.GetRequiredService<TaskCompletionSource<T>>();

        return builder =>
        {
            builder.OpenComponent<T>(0);
            builder.AddComponentReferenceCapture(1, tool => taskSource.TrySetResult((T)tool));
            builder.CloseComponent();
        };
    }
}