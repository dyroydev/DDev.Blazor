using ScheduleApp.Services;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public class EventEditModel : EditModel<EventVm>
{
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

    public string? Description
    {
        get => Get(x => x.Description);
        set => Set(x => x.Description, value);
    }

    public string? Location
    {
        get => Get(x => x.Location);
        set => Set(x => x.Location, value);
    }

    public DateOnly? Date
    {
        get => DateOnly.FromDateTime(Get(x => x.Repeater).Start.DateTime);
        set
        {
            var date = value ?? DateOnly.MinValue;
            var repeater = Model?.Repeater ?? Repeater.Never;
            repeater.Start = new DateTimeOffset(date.Year, date.Month, date.Day, repeater.Start.Hour, repeater.Start.Minute, repeater.Start.Second, repeater.Start.Offset);
            Set(x => x.Repeater, repeater);
        }
    }

    public TimeOnly? Time
    {
        get => TimeOnly.FromDateTime(Get(x => x.Repeater).Start.DateTime);
        set
        {
            var time = value ?? TimeOnly.MinValue;
            var repeater = Model?.Repeater ?? Repeater.Never;
            repeater.Start = new DateTimeOffset(repeater.Start.Year, repeater.Start.Month, repeater.Start.Day, time.Hour, time.Minute, time.Second, repeater.Start.Offset);
            Set(x => x.Repeater, repeater);
        }
    }

    public int RepeatPeriod
    {
        get => Get(x => x.Repeater).Period;
        set
        {
            var repeater = Model?.Repeater ?? Repeater.Never;
            repeater.Period = value;
            Set(x => x.Repeater, repeater);
        }
    }

    public RepeaterType RepeatType
    {
        get => Get(x => x.Repeater).Type;
        set
        {
            var repeater = Model?.Repeater ?? Repeater.Never;
            repeater.Type = value;
            Set(x => x.Repeater, repeater);
        }
    }

    [Required]
    public ScheduleVm? Schedule
    {
        get; set;
    }
}
