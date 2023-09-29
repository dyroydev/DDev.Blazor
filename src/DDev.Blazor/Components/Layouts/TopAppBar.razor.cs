namespace DDev.Blazor.Components.Layouts;

/// <summary>
/// A application bar on top of the page.
/// </summary>
public partial class TopAppBar
{
    /// <summary>
    /// Content anchored to the left of the app-bar.
    /// </summary>
    [Parameter] public RenderFragment? Left { get; set; }

    /// <summary>
    /// Page title.
    /// </summary>
    [Parameter] public RenderFragment? Title { get; set; }

    /// <summary>
    /// Content anchored to the right of the app-bar.
    /// </summary>
    [Parameter] public RenderFragment? Right { get; set; }

    /// <summary>
    /// Details wich can be hidden or shown.
    /// </summary>
    [Parameter] public RenderFragment? Details { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the app-bar is raised above the other page elements.
    /// </summary>
    [Parameter] public bool Raised { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the details are visible.
    /// </summary>
    [Parameter] public bool Expanded { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the app-bar is rendered using the primary colors.
    /// </summary>
    [Parameter] public bool Primary { get; set; }
}
