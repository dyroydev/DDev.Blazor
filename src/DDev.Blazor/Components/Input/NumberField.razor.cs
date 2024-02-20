using System.Numerics;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// A field for numeric values.
/// </summary>
/// <typeparam name="T">Numeric type of value.</typeparam>
/// <seealso cref="DateField"/>
/// <seealso cref="SelectField{T}"/>
/// <seealso cref="TextArea"/>
/// <seealso cref="TextField"/>
public partial class NumberField<T> where T : INumber<T>, IMinMaxValue<T>
{
    /// <summary>
    /// Value is set to this if field is blank.
    /// </summary>
    /// <remarks>
    /// Default is zero.
    /// </remarks>
    [Parameter] public T Default { get; set; } = T.Zero;

    /// <summary>
    /// The minimum allowed value.
    /// </summary>
    [Parameter] public T Min { get; set; } = T.MinValue;

    /// <summary>
    /// The maximum allowed value.
    /// </summary>
    [Parameter] public T Max { get; set; } = T.MaxValue;

    /// <summary>
    /// The amount value is incremented or decremented by spinner buttons.
    /// </summary>
    [Parameter] public T Step { get; set; } = T.One;

    private string _valueAsString = "";

    private async Task HandleStringValueChanged()
    {
        if (string.IsNullOrWhiteSpace(_valueAsString))
            await base.SetValueAsync(Default);

        else if (T.TryParse(_valueAsString, null, out var value))
            await base.SetValueAsync(value);

        else
            await base.SetValueAsync(Default);
    }

    /// <inheritdoc />
    protected override async Task SetValueAsync(T? value)
    {
        if (Equals(value, Value))
            return;

        await base.SetValueAsync(value);
        _valueAsString = Format(value);
    }

    private string Format(T? value)
    {
        return value?.ToString() ?? "";
    }
}
