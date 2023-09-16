namespace DDev.Blazor.Components.Layouts;

public partial class TopAppBar
{
    [Parameter] public RenderFragment? LeftActions { get; set; }
    [Parameter] public RenderFragment? Title { get; set; }
    [Parameter] public RenderFragment? RightActions { get; set; }
    [Parameter] public RenderFragment? Content { get; set; }
    [Parameter] public bool Raised { get; set; }
    [Parameter] public bool Expanded { get; set; }
    [Parameter] public bool Primary { get; set; }
}
