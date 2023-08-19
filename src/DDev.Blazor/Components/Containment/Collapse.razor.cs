namespace DDev.Blazor.Components.Containment;

/// <summary>
/// Can collapse and expand content smoothly.
/// </summary>
public partial class Collapse
{
    /// <summary>
    /// If <see langword="true"/>, the collapse is expanded.
    /// </summary>
    [Parameter] public bool Expanded { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the collapse expands and collapses inline (horizontally).
    /// </summary>
    [Parameter] public bool Inline { get; set; }

    /// <summary>
    /// Content in collapse.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}