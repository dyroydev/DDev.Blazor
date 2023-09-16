namespace DDev.Blazor.Components.Input;

public partial class DatePicker
{
    [Parameter] public DateOnly Month { get; set; }

    [Parameter] public EventCallback<DateOnly> MonthChanged { get; set; }

    [Parameter] public Func<DateOnly, string?>? DateTitle { get; set; } = date => date == _today ? "Today" : null;

    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Now);
    private readonly int _dayCount = 7 * 6;
    private DateOnly _startDay;

    private bool _isDatesVisible = true;
    private bool _isMonthsVisible;
    private bool _isYearsVisible;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (Month == DateOnly.MinValue)
            await SetMonthAsync(_today);
    }

    /// <inheritdoc/>
    protected override async Task OnValueChangedAsync()
    {
        await SetMonthAsync(Value ?? _today);
    }

    /// <inheritdoc/>
    public override async Task FocusAsync()
    {
        if (_isDatesVisible)
            await Js.FocusAsync(GetDateId(Month));
        else if (_isMonthsVisible)
            await Js.FocusAsync(GetMonthId(Month));
        else if (_isYearsVisible)
            await Js.FocusAsync(GetYearId(Month));
        else
            await Js.FocusAsync(Id + "__previous-month");
    }

    /// <summary>
    /// Sets the month to the given value.
    /// </summary>
    public async Task SetMonthAsync(DateOnly month)
    {
        if (month == DateOnly.MinValue)
            month = _today;

        month = new DateOnly(month.Year, month.Month, 1);

        if (Month == month)
            return;

        _startDay = month.GetStartOfWeek();
        Month = month;
        await MonthChanged.InvokeAsync(month);
        StateHasChanged();
    }

    private void ToggleMonths()
    {
        _isYearsVisible = false;
        _isMonthsVisible = !_isMonthsVisible;
        _isDatesVisible = !_isMonthsVisible;
    }

    private void ToggleYears()
    {
        _isMonthsVisible = false;
        _isYearsVisible = !_isYearsVisible;
        _isDatesVisible = !_isYearsVisible;
    }

    private async Task PreviousMonth()
    {
        await SetMonthAsync(Month.AddMonths(-1));
    }

    private async Task NextMonth()
    {
        await SetMonthAsync(Month.AddMonths(1));
    }

    private async Task PreviousYear()
    {
        await SetMonthAsync(Month.AddYears(_isYearsVisible ? -10 : -1));
    }

    private async Task NextYear()
    {
        await SetMonthAsync(Month.AddYears(_isYearsVisible ? 10 : 1));
    }

    private async Task ClickPicker()
    {
        if (await Js.HasFocusAsync(Id))
            return;

        await FocusAsync();
    }

    private async Task ClickDate(DateOnly date)
    {
        await SetValueAsync(date);
    }

    private async Task ClickMonth(DateOnly month)
    {
        _isDatesVisible = true;
        _isMonthsVisible = false;
        _isYearsVisible = false;
        await SetMonthAsync(month);
    }

    private bool IsDateSelected(DateOnly date)
    {
        return date == Value;
    }

    private string GetDateTitle(DateOnly date)
    {
        return DateTitle?.Invoke(date) ?? date.ToShortDateString();
    }

    private string GetDateId(DateOnly date) => $"{Id}__day-{date.DayNumber}";
    private string GetMonthId(DateOnly date) => $"{Id}__month-{date.Month}";
    private string GetYearId(DateOnly date) => $"{Id}__year-{date.Year}";
}
