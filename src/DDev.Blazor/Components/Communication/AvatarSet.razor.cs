namespace DDev.Blazor.Components.Communication;

/// <summary>
/// A set of <see cref="Avatar"/> components.
/// </summary>
public partial class AvatarSet
{
    /// <summary>
    /// Should contain <see cref="Avatar"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}