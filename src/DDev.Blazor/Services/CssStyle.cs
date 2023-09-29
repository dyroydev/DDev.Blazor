namespace DDev.Blazor.Services;

/// <summary>
/// Utility to simplify building css styles.
/// </summary>
public readonly struct CssStyle
{
    private readonly StringBuilder _sb;

    /// <summary>
    /// Creates a new css-style.
    /// </summary>
    public CssStyle()
    {
        _sb = new();
    }

    /// <summary>
    /// Sets property <paramref name="name"/> to <paramref name="value"/>.
    /// </summary>
    public CssStyle Set(string name, object? value)
    {
        if (value is null)
            return this;

        _sb.Append(name).Append(':').Append(value).Append(';');
        return this;
    }

    /// <summary>
    /// Sets property <paramref name="name"/> to <paramref name="value"/> if condition <paramref name="when"/> is <see langword="true"/>.
    /// </summary>
    public CssStyle Set(string name, object? value, bool when)
    {
        return Set(name, when ? value : null);
    }

    /// <summary>
    /// Sets property <paramref name="name"/> to value returned by <paramref name="valueFactory"/> if condition <paramref name="when"/> is <see langword="true"/>.
    /// </summary>
    public CssStyle Set(string name, Func<object?> valueFactory, bool when = true)
    {
        return Set(name, when ? valueFactory() : null);
    }

    /// <summary>
    /// Adds a CSS style and a semicolon.
    /// </summary>
    /// <param name="style">CSS style.</param>
    public CssStyle Add(object? style)
    {
        if (style is not null)
            _sb.Append(style).Append(';');
        return this;
    }

    /// <summary>
    /// Adds styles from <c>"style"</c>-key if exists.
    /// </summary>
    public CssStyle Add(IReadOnlyDictionary<string, object?>? attributes)
    {
        return Add(attributes?.GetValueOrDefault("style"));
    }

    /// <summary>
    /// Returns the string-representation of the css style.
    /// </summary>
    public override string ToString()
    {
        return _sb.ToString();
    }
}