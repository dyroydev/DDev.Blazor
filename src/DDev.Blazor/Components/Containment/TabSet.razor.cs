namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A set of <see cref="Containment.Tab"/> components.
/// </summary>
public partial class TabSet
{
    /// <summary>
    /// Should contain <see cref="Containment.Tab"/> components.
    /// </summary>
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    internal HashSet<Tab> Tabs { get; } = new();

    /// <summary>
    /// The currently open <see cref="Containment.Tab"/>. <see langword="null"/> if no tab is open.
    /// </summary>
    public Tab? Tab { get; private set; }

    /// <summary>
    /// Opens the given <paramref name="tab"/>.
    /// </summary>
    /// <param name="tab">Tab to open.</param>
    /// <returns>A <see cref="Task"/> that completes when <paramref name="tab"/> is open.</returns>
    /// <exception cref="InvalidOperationException">If <paramref name="tab"/> is not a part of the tab-set.</exception>
    public Task OpenTabAsync(Tab? tab)
    {
        if (Tabs.Contains(tab!) is false)
            throw new InvalidOperationException($"Given {nameof(Containment.Tab)} is not part of the {nameof(TabSet)}");

        if (Tab == tab)
            return Task.CompletedTask;

        Tab = tab;
        StateHasChanged();
        return Task.CompletedTask;
    }
}
