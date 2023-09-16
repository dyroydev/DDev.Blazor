namespace DDev.Blazor.Components.Containment;

/// <summary>
/// A named section of content in a tab-set.
/// </summary>
public partial class Tab : IDisposable
{
    /// <summary>
    /// Title identifying tab.
    /// </summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// Icon identifying tab.
    /// </summary>
    [Parameter] public string? Icon { get; set; }

    /// <summary>
    /// Content in tab.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] private TabSet? TabSet { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        TabSet?.Tabs.Add(this);
        if (TabSet is not null && TabSet.Tab is null)
            await TabSet.OpenTabAsync(this);
    }

    void IDisposable.Dispose()
    {
        TabSet?.Tabs.Remove(this);
    }

    private async Task HandleClick()
    {
        await TabSet!.OpenTabAsync(this);
    }
}