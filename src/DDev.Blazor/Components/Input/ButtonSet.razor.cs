namespace DDev.Blazor.Components.Input;

/// <summary>
/// A set of <see cref="Button"/> components.
/// </summary>
public partial class ButtonSet
{
    /// <summary>
    /// Should contain <see cref="Button"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}