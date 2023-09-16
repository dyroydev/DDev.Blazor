namespace DDev.Blazor.Components.Selection;

[CascadingTypeParameter(nameof(T))]
public partial class OptionSource<T>
{
    /// <summary>
    /// A collection of items.
    /// </summary>
    [Parameter, EditorRequired] public IEnumerable<T> Source { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Callback invoked when an option is clicked.
    /// </summary>
    [Parameter] public EventCallback<T> OnClick { get; set; }

    /// <summary>
    /// Callback invoked when a hint is clicked.
    /// </summary>
    [Parameter] public EventCallback<T> OnClickHint { get; set; }

    /// <summary>
    /// A function that determines if an item is disabled.
    /// </summary>
    [Parameter] public Func<T, bool> Disabled { get; set; } = AlwaysFalse;
    /// <summary>
    /// A function that determines if an item is disabled.
    /// </summary>
    [Parameter] public Func<T, string?> Hint { get; set; } = AlwaysNull;
    /// <summary>
    /// A function that determines if an item is disabled.
    /// </summary>
    [Parameter] public Func<T, string?> HintIcon { get; set; } = AlwaysNull;
    /// <summary>
    /// A function that determines if an item is disabled.
    /// </summary>
    [Parameter] public Func<T, string?> Icon { get; set; } = AlwaysNull;

    /// <summary>
    /// Optional template for items.
    /// </summary>
    [Parameter] public RenderFragment<T> ChildContent { get; set; } = value => builder => builder.AddContent(0, value?.ToString());

    private EventCallback GetCallback(EventCallback<T> callback, T item)
    {
        if (callback.HasDelegate is false)
            return default;

        return EventCallback.Factory.Create(this, () => callback.InvokeAsync(item));
    }

    private static bool AlwaysFalse(T value) => false;

    private static string? AlwaysNull(T value) => null;
}
