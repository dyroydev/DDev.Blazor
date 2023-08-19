using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDev.Blazor.Components.Containment;

public partial class TabSet
{
    [Parameter, EditorRequired] public RenderFragment? ChildContent { get; set; }

    internal HashSet<Tab> Tabs { get; } = new();

    /// <summary>
    /// The currently open <see cref="Containment.Tab"/>. <see langword="null"/> if no tab is open.
    /// </summary>
    public Tab? Tab { get; private set; }

    public Task SetTabAsync(Tab? tab)
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
