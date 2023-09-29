using Microsoft.AspNetCore.Components.Web;

namespace DDev.Blazor.Components.Input.Custom;

/// <summary>
/// A template for making input-field components. Works well with <see cref="FieldBase{T}"/>
/// </summary>
/// <seealso cref="FieldBase{T}"/>
public partial class FieldTemplate
{
    /// <summary>
    /// If <see langword="true"/>, the field is considered empty.
    /// </summary>
    [Parameter] public bool Empty { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the field is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the field is rendered as focused.
    /// </summary>
    /// <remarks>
    /// Usefull when field temporarily delegates focus to a popup or similar.
    /// </remarks>
    [Parameter] public bool ForceFocused { get; set; }

    /// <summary>
    /// Defines a label describing the value.
    /// </summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// If provided, should be id of value element. 
    /// </summary>
    [Parameter] public string? ValueId { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the field is invalid.
    /// </summary>
    [Parameter] public bool Invalid { get; set; }

    /// <summary>
    /// Id of field element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    /// <summary>
    /// Callback invoked when the field is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Display value of field.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Enter" || args.Key == "Space")
            await HandleClick();
    }

    private async Task HandleClick()
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();

        else if (await Js.HasFocusAsync(Id) is false)
            await Js.FocusAsync(ValueId);
    }
}
