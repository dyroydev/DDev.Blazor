namespace DDev.Blazor.Components.Input;

/// <summary>
/// An input component which holds a <see cref="string"/> value.
/// </summary>
public partial class TextField
{
    /// <summary>
    /// The placeholder contains hints for what a input should look like.
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// Defines which virtial keyboard is most appropriate for the field.
    /// </summary>
    [Parameter] public InputMode InputMode { get; set; }
}