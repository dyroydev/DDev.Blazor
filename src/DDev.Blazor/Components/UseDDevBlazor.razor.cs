namespace DDev.Blazor.Components;

/// <summary>
/// Main entry point for DDev Blazor. Must be added to top of &lt;App/&gt; component.
/// </summary>
public partial class UseDDevBlazor
{
    /// <summary>
    /// Renders an internal component.
    /// </summary>
    private RenderFragment UseInternal<T>() where T : IComponent
    {
        return builder =>
        {
            builder.OpenComponent<T>(0);
            builder.CloseComponent();
        };
    }
}