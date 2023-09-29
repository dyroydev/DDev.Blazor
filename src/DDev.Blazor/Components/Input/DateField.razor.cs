using System.Globalization;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A field for <see cref="DateOnly"/>? values.
/// </summary>
public partial class DateField
{
    /// <summary>
    /// A list of supported date formats.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/> or empty, the supported formats are inferred from <see cref="CultureInfo.CurrentCulture"/>.
    /// </remarks>
    public static string[]? SupportedFormats { get; set; }

    /// <summary>
    /// The preferred date format.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/> or white-space, the preferred format is the short-date format from <see cref="CultureInfo.CurrentCulture"/>.
    /// </remarks>
    public static string? PreferredFormat { get; set; }

    private string Placeholder => PreferredFormat ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

    private readonly string _fieldId = ComponentId.New();
    private string _valueAsString = "";
    private Popup? _popup;
    private DatePicker? _picker;

    /// <summary>
    /// Shows the picker if not already visible. <br/>
    /// Delegates focus to the input.
    /// </summary>
    public async Task ShowPickerAsync()
    {
        if (_popup is null)
            return;

        await _popup.ShowAsync(_fieldId);
        await FocusAsync();
    }

    private async Task HandleStringValueChanged()
    {
        if (string.IsNullOrWhiteSpace(_valueAsString))
            await base.SetValueAsync(null);

        else if (TryParse(_valueAsString, out var value))
            await base.SetValueAsync(value);

        else
            await base.SetValueAsync(null);
    }

    /// <inheritdoc />
    public override async Task FocusAsync()
    {
        if (_popup?.IsVisible is not true)
            await base.FocusAsync();

        else if (_picker is not null)
            await _picker.FocusAsync();
    }

    /// <inheritdoc />
    protected override async Task SetValueAsync(DateOnly? value)
    {
        if (Equals(value, Value))
            return;

        await base.SetValueAsync(value);
        _valueAsString = Format(value);
    }

    private string Format(DateOnly? value)
    {
        if (value is null || value == DateOnly.MinValue)
            return "";

        if (string.IsNullOrWhiteSpace(PreferredFormat))
            return value!.Value.ToShortDateString();

        return value!.Value.ToDateTime(TimeOnly.MinValue).ToString(PreferredFormat);
    }

    private static bool TryParse(string value, out DateOnly? date)
    {
        var supportedFormats = SupportedFormats is { Length: > 0 }
            ? SupportedFormats
            : CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();

        if (DateTime.TryParseExact(value, supportedFormats, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var dateTime))
        {
            date = DateOnly.FromDateTime(dateTime);
            return true;
        }

        date = null;
        return false;
    }
}