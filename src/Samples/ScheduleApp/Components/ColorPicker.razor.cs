using Microsoft.AspNetCore.Components;

namespace ScheduleApp.Components;

public partial class ColorPicker
{
    [Parameter] public int ColorCount { get; set; } = 27;

    private string GetColor(int index)
    {
        var hue = index * 360 / ColorCount;
        return $"hsl({hue}, 65%, 65%)";
    }
}
