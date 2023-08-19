namespace DDev.Blazor.Components;

public partial class DDevBlazor
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