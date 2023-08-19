namespace DDev.Blazor.Components.Selection;

public partial class Option
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Icon { get; set; }

    [Parameter] public string? Hint { get; set; }

    [Parameter] public string? HintIcon { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    [Parameter] public bool Disabled { get; set; }

    private async Task HandleClick()
    {
        if (Disabled)
            return;

        await OnClick.InvokeAsync();
    }
}