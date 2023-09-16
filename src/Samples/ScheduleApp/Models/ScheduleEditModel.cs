using ScheduleApp.Services;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public class ScheduleEditModel : EditModel<ScheduleVm>
{
    [Required]
    public string Name
    {
        get => Get(x => x.Name);
        set => Set(x => x.Name, value);
    }


    public string? Color
    {
        get => Get(x => x.Color);
        set => Set(x => x.Color, value);
    }
}