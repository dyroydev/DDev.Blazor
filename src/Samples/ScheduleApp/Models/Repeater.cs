namespace ScheduleApp.Models;

public class Repeater
{
    public static Repeater Never => new Repeater();

    public static Repeater Daily(DateTimeOffset start, int period = 1) => new () { Start = start, Period = period, Type = RepeaterType.Daily };
    
    public static Repeater Weekly(DateTimeOffset start, int period = 1) => new () { Start = start, Period = period * 7, Type = RepeaterType.Daily };

    public static Repeater Monthly(DateTimeOffset start, int period = 1) => new () { Start = start, Period = period, Type = RepeaterType.Monthly };

    public static Repeater Quarterly(DateTimeOffset start, int period = 1) => new() { Start = start, Period = period * 3, Type = RepeaterType.Monthly };

    public static Repeater Yearly(DateTimeOffset start, int period = 1) => new() { Start = start, Period = period, Type = RepeaterType.Yearly };

    public DateTimeOffset Start { get; set; }

    public RepeaterType Type { get; set; }

    public DateTimeOffset? End { get; set; }

    public int Period { get; set; } = 1;

    public List<DateTimeOffset> Cancels { get; set; } = new();

    public IEnumerable<DateTimeOffset> GetBetween(DateTimeOffset start, DateTimeOffset end)
    {
        if (Start > end || End.HasValue && End.Value < start)
            yield break;

        if (Type is RepeaterType.Never)
        {
            yield return Start;
            yield break;
        }

        var date = Start;

        while (date < start)
            date = NextCandidate(date);

        while (date < end)
        {
            yield return date;
            date = NextCandidate(date);
        }
    }

    private DateTimeOffset NextCandidate(DateTimeOffset date) => Type switch
    {
        RepeaterType.Daily => date.AddDays(Period),
        RepeaterType.Monthly => date.AddMonths(Period),
        RepeaterType.Yearly => date.AddYears(Period),
        _ => throw new NotImplementedException()
    };
}

public enum RepeaterType
{
    Never,
    Daily,
    Monthly,
    Yearly
}