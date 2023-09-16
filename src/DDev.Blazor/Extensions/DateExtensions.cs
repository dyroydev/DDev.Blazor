using System.Globalization;

namespace DDev.Blazor.Extensions;

/// <summary>
/// Usefull extensions for date-objects when working with schedules.
/// </summary>
public static class DateExtensions
{
    /// <summary>
    /// Returns the first date in the week that contains <paramref name="date"/>.
    /// </summary>
    public static DateOnly GetStartOfWeek(this DateOnly date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        while (date.DayOfWeek != culture.DateTimeFormat.FirstDayOfWeek)
            date = date.AddDays(-1);
        return date;
    }

    /// <summary>
    /// Returns the first date in the month that contains <paramref name="date"/>.
    /// </summary>
    public static DateOnly GetStartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Returns the week of year the given <paramref name="date"/> is in.
    /// </summary>
    public static int GetWeekOfYear(this DateOnly date, CultureInfo? culture = null)
    {
        return GetWeekNumber(date.ToDateTime(TimeOnly.MinValue), culture);
    }

    /// <summary>
    /// Returns the week of year the given <paramref name="date"/> is in.
    /// </summary>
    public static int GetWeekNumber(this DateTime date, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
    }

    /// <summary>
    /// Returns the first date in the decade that contains <paramref name="date"/>.
    /// </summary>
    public static DateOnly GetStartOfDecade(this DateOnly date)
    {
        var year = (date.Year / 10) * 10;
        return new DateOnly(year, 1, 1);
    }
}
