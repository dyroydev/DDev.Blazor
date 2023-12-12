using DDev.Blazor.Components.Selection;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A combination a <see cref="TextField"/> and a <see cref="SelectField{T}"/>, allowing users to select an item from the list or possibly enter a custom value.
/// </summary>
/// <typeparam name="T">Type of item in the combo-field.</typeparam>
public partial class ComboField<T>
{
    /// <summary>
    /// If supplied, the user can select from this range of values.
    /// </summary>
    [Parameter] public IEnumerable<T>? Source { get; set; }

    /// <summary>
    /// Function that formats the value as a string. Default is <see cref="object.ToString"/>.
    /// </summary>
    [Parameter] public Func<T?, string>? Format { get; set; }

    /// <summary>
    /// If supplied, the user can enter a custom value. Otherwise the value is <see langword="default"/> when the text does not match any items in the source.
    /// </summary>
    [Parameter] public Func<string, T?>? Custom { get; set; }

    /// <summary>
    /// Template for options in the source.
    /// </summary>
    [Parameter] public RenderFragment<T>? ChildContent { get; set; }

    private PopupMenu? _popup;
    private bool _updateValueAsString = true;
    private string _valueAsString = "";
    private List<T> _candidates = [];

    private async Task HandleValueAsStringChanged()
    {
        _candidates = (Source ?? []).Where(item => AsString(item).StartsWith(_valueAsString)).ToList();

        if (_candidates is [var onlyCandidate])
        {
            await SetValueAsync(onlyCandidate);
        }
        else if (Custom is not null)
        {
            _updateValueAsString = false;
            await SetValueAsync(Custom(_valueAsString));
        }
        else
        {
            await SetValueAsync(default);
        }
    }

    /// <inheritdoc />
    protected sealed override void OnValueChanged()
    {
        if (_updateValueAsString)
            _valueAsString = AsString(Value);
        else
            _updateValueAsString = true;
    }

    private string AsString(T? item) => Format?.Invoke(item) ?? item?.ToString() ?? "";

    private async Task HandleFocus()
    {
        if (_popup is { IsVisible: false })
        {
            await _popup.ShowAsync(Id);
        }
    }

    private async Task HandleBlur()
    {
        // Give browser time to delegate the focus to the popup if needed.
        await Task.Delay(10);

        if (_popup is { IsVisible: true })
        {
            await _popup.CloseAsync();
        }
    }
}
