namespace DDev.Blazor.Components.Containment;

public partial class Tab : IDisposable
{
    [Parameter] public string? Title { get; set; }

    [Parameter] public string? Icon { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] private TabSet? TabSet { get; set; }

    protected override async Task OnInitializedAsync()
    {
        TabSet?.Tabs.Add(this);
        if (TabSet is not null && TabSet.Tab is null)
            await TabSet.SetTabAsync(this);
    }

    void IDisposable.Dispose()
    {
        TabSet?.Tabs.Remove(this);
    }

    private async Task HandleClick()
    {
        await TabSet!.SetTabAsync(this);
    }
}