using DDev.Blazor.Services;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A picker that lets the user select a date.
/// </summary>
public partial class DatePicker
{
    /// <summary>
    /// The currently visible month.
    /// </summary>
    [Parameter] public DateOnly Month { get; set; }

    /// <summary>
    /// Callback invoked when the currently visible month has changed.
    /// </summary>
    [Parameter] public EventCallback<DateOnly> MonthChanged { get; set; }

    /// <summary>
    /// Optional function to give a special title to a date.
    /// </summary>
    /// <remarks>
    /// If the returned value is a <see cref="string"/>, the value is shown when the user hovers or focuses the date.<br/>
    /// If the returned value is <see langword="null"/>, the string-representation of the date is shown instead.
    /// </remarks>
    [Parameter] public Func<DateOnly, string?>? DateTitle { get; set; } = date => date == _today ? "Today" : null;

    [Inject] private IKeyBindingsFactory KeyBinds { get; set; } = null!;

    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Now);
    private readonly List<IDisposable> _keyBinds = new();
    private readonly int _dayCount = 7 * 6;
    private readonly int _monthCount = 12;
    private readonly int _yearCount = 15;
    private DateOnly _startDay;
    private DateOnly _startMonth;
    private DateOnly _startYear;

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

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _keyBinds.ForEach(x => x.Dispose());
        _keyBinds.Clear();
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
        {
            // State has some-how become invalid.
            ShowDates();
            await Js.FocusAsync(GetDateId(Month));
        }
    }

    /// <summary>
    /// Delegates focus to the given date.
    /// </summary>
    public async Task FocusAsync(DateOnly date)
    {
        ShowDates();

        // Wait for dates to render.
        await Task.Yield();

        if (date <  _startDay || date > _startDay.AddDays(_dayCount))
            await SetMonthAsync(date);

        await Js.FocusAsync(GetDateId(date));
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
        _startMonth = month.GetStartOfYear();
        _startYear = month.GetStartOfDecade();
        Month = month;
        StateHasChanged();
        SetupKeyBindings();

        await MonthChanged.InvokeAsync(month);
    }

    private void ShowDates()
    {
        _isDatesVisible = true;
        _isMonthsVisible = false;
        _isYearsVisible = false;
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
        ShowDates();
        await SetMonthAsync(month);
        await Js.FocusAsync(Id + "__toggle-month");
    }

    private async Task ClickYear(DateOnly year)
    {
        ShowDates();
        await SetMonthAsync(year);
        await Js.FocusAsync(Id + "__toggle-year");
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

    private void SetupKeyBindings()
    {
        _keyBinds.ForEach(x => x.Dispose());
        _keyBinds.Clear();

        _keyBinds.Add(KeyBinds.Create(Id)
            .On("Delete", () => SetValueAsync(null))
            .On("Home", ModifierKey.Shift, () => FocusAsync(_today)));

        for (var i = 0; i < _dayCount; i++)
        {
            var date = _startDay.AddDays(i);
            var keyBinds = KeyBinds.Create(GetDateId(date))
                .On("ArrowUp", () => FocusAsync(date.AddDays(-7)))
                .On("ArrowDown", () => FocusAsync(date.AddDays(7)))
                .On("ArrowLeft", () => FocusAsync(date.AddDays(-1)))
                .On("ArrowRight", () => FocusAsync(date.AddDays(1)))
                .On("Home", () => FocusAsync(date.GetStartOfWeek()))
                .On("End", () => FocusAsync(date.GetStartOfWeek().AddDays(6)))
                .On("PageUp", () => FocusAsync(date.AddMonths(-1)))
                .On("PageDown", () => FocusAsync(date.AddMonths(1)))
                .On("PageUp", ModifierKey.Shift, () => FocusAsync(date.AddYears(-1)))
                .On("PageDown", ModifierKey.Shift, () => FocusAsync(date.AddYears(1)))
                .On("PageUp", ModifierKey.Shift | ModifierKey.Control, () => FocusAsync(date.AddYears(-10)))
                .On("PageDown", ModifierKey.Shift | ModifierKey.Control, () => FocusAsync(date.AddYears(10)))
                .On(" ", () => ClickDate(date))
                .On("Enter", () => ClickDate(date));

            _keyBinds.Add(keyBinds);
        }

        for (var i = 0; i < _monthCount; i++)
        {
            var date = _startDay.AddMonths(i);
            var keyBinds = KeyBinds.Create(GetMonthId(date))
                .On("ArrowUp", () => Js.FocusAsync(GetMonthId(date.AddMonths(-3))))
                .On("ArrowDown", () => Js.FocusAsync(GetMonthId(date.AddMonths(3))))
                .On("ArrowLeft", () => Js.FocusAsync(GetMonthId(date.AddMonths(-1))))
                .On("ArrowRight", () => Js.FocusAsync(GetMonthId(date.AddMonths(1))))
                .On(" ", () => ClickMonth(date))
                .On("Enter", () => ClickMonth(date));

            _keyBinds.Add(keyBinds);
        }

        for (var i = 0; i < _yearCount; i++)
        {
            var date = _startDay.AddYears(i);
            var keyBinds = KeyBinds.Create(GetYearId(date))
                .On("ArrowUp", () => Js.FocusAsync(GetYearId(date.AddYears(-3))))
                .On("ArrowDown", () => Js.FocusAsync(GetYearId(date.AddYears(3))))
                .On("ArrowLeft", () => Js.FocusAsync(GetYearId(date.AddYears(-1))))
                .On("ArrowRight", () => Js.FocusAsync(GetYearId(date.AddYears(1))))
                .On(" ", () => ClickYear(date))
                .On("Enter", () => ClickYear(date));

            _keyBinds.Add(keyBinds);
        }
    }
}
