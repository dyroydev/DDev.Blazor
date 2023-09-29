using DDev.Blazor.Services;
using System.Globalization;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A field for <see cref="TimeOnly"/>? values.
/// </summary>
public partial class TimeField
{
    /// <summary>
    /// If <see langword="true"/>, seconds is hidden and always set to <c>0</c>.
    /// </summary>
    [Parameter] public bool NoSeconds { get; set; }

    [Inject] IKeyBindingsFactory KeyBinds { get; set; } = null!;

    private string Separator => CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
    private string HourId => Id + "_hour";
    private string MinuteId => Id + "_minute";
    private string SecondId => Id + "_second";
    private bool IsEmpty => string.IsNullOrWhiteSpace(_hourAsString) && string.IsNullOrWhiteSpace(_minuteAsString) && string.IsNullOrWhiteSpace(_secondAsString);

    private string _hourAsString = "";
    private string _minuteAsString = "";
    private string _secondAsString = "";

    private int _hour;
    private int _minute;
    private int _second;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Use(KeyBinds.ForElement(HourId))
            .On("ArrowUp", () => SetValueAsync((Value ?? TimeOnly.MinValue).AddHours(1)))
            .On("ArrowDown", () => SetValueAsync((Value ?? TimeOnly.MinValue).AddHours(-1)))
            .On("ArrowLeft", () => NoSeconds ? FocusMinuteAsync() : FocusSecondAsync())
            .On("ArrowRight", () => FocusMinuteAsync());

        Use(KeyBinds.ForElement(MinuteId))
            .On("ArrowUp", () => SetValueAsync((Value ?? TimeOnly.MinValue).AddMinutes(1)))
            .On("ArrowDown", () => SetValueAsync((Value ?? TimeOnly.MinValue).AddMinutes(-1)))
            .On("ArrowLeft", () => FocusHourAsync())
            .On("ArrowRight", () => NoSeconds ? FocusHourAsync() : FocusSecondAsync());

        Use(KeyBinds.ForElement(SecondId))
            .On("ArrowUp", () => SetValueAsync((Value ?? TimeOnly.MinValue).Add(TimeSpan.FromSeconds(1))))
            .On("ArrowDown", () => SetValueAsync((Value ?? TimeOnly.MinValue).Add(TimeSpan.FromSeconds(-1))))
            .On("ArrowLeft", () => FocusMinuteAsync())
            .On("ArrowRight", () => FocusHourAsync());
    }

    /// <inheritdoc />
    public override async Task FocusAsync()
    {
        await FocusHourAsync();
    }

    /// <inheritdoc />
    protected override void OnValueChanged()
    {
        _hour = Value?.Hour ?? 0;
        _minute = Value?.Minute ?? 0;
        _second = Value?.Second ?? 0;

        _hourAsString = Format(_hour);
        _minuteAsString = Format(_minute);
        _secondAsString = Format(_second);
    }

    private async Task HourChangedAsync()
    {
        if (_hourAsString.Contains(':'))
        {
            _hourAsString = _hourAsString.Split(':')[0];
            await FocusMinuteAsync();
        }

        _hour = ParseOrDefault(_hourAsString, max: 59);

        await ValueChangedAsync();
    }

    private async Task MinuteChangedAsync()
    {
        if (_minuteAsString.Contains(':') && NoSeconds is false)
        {
            _minuteAsString = _minuteAsString.Split(':')[0];
            await FocusSecondAsync();
        }

        _minute = ParseOrDefault(_minuteAsString, max:59);

        await ValueChangedAsync();
    }

    private async Task SecondChangedAsync()
    {
        _second = ParseOrDefault(_secondAsString, max:59);
        await ValueChangedAsync();
    }

    private async Task FocusHourAsync()
    {
        await Js.InvokeDDevAsync("element", "invokeMethod", HourId, "select");
    }

    private async Task FocusMinuteAsync()
    {
        await Js.InvokeDDevAsync("element", "invokeMethod", MinuteId, "select");
    }

    private async Task FocusSecondAsync()
    {
        await Js.InvokeDDevAsync("element", "invokeMethod", SecondId, "select");
    }

    private async Task ValueChangedAsync()
    {
        await SetValueAsync(IsEmpty ? null : new TimeOnly(_hour, _minute, _second));
    }

    private int ParseOrDefault(string text, int max) => Math.Clamp(int.TryParse(text, out var value) ? value : default, 0, max);

    private string Format(int value) => value is 0 ? "" : value.ToString("00");
}