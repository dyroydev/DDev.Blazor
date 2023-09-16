using Microsoft.AspNetCore.Components.Web;

namespace DDev.Blazor.Components.Selection;

/// <summary>
/// A list of selectable action-items represented by <see cref="Option"/> components.
/// </summary>
public partial class Menu
{
    /// <summary>
    /// Menu content. Should be <see cref="Option"/> components or <c>&lt;hr/&gt;</c>-elements.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Value for <c>id</c>-attribute on menu element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    /// <summary>
    /// Callback invoked when the menu is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    /// <summary>
    /// Callback invoked when a key is pressed.
    /// </summary>
    [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

    /// <summary>
    /// Callback invoked when an option is clicked.
    /// </summary>
    [Parameter] public EventCallback<Option> OnClickOption { get; set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    /// <summary>
    /// A set containing all options within the menu.
    /// </summary>
    /// <remarks>The set does not gurranty correct or stable order.</remarks>
    public IReadOnlySet<Option> Options => InternalOptions;

    internal HashSet<Option> InternalOptions { get; } = new();

    /// <summary>
    /// A set of options that is always empty.
    /// </summary>
    internal static IReadOnlySet<Option> EmptyOptions { get; } = new HashSet<Option>();

    /// <summary>
    /// Gives focus to the first item in the menu.
    /// </summary>
    public async Task FocusAsync()
    {
        await Js.FocusAsync($"#{Id} li:not([disabled])");
    }

    /// <summary>
    /// Notifies that the option was clicked.
    /// </summary>
    internal async Task NotifyClickedAsync(Option option)
    {
        await OnClickOption.InvokeAsync(option);
    }
}