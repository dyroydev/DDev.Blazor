using DDev.Blazor.Components.Selection;

namespace DDev.Blazor.Components.Input;

[CascadingTypeParameter(nameof(T))]
public partial class SelectField<T>
{
    /// <summary>
    /// Should contain <see cref="Option"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private readonly string _fieldId = ComponentId.New();
    private PopupMenu? _menu;

    private async Task ClickAsync()
    {
        if (_menu is null)
            return;

        await _menu.ShowAsync(_fieldId);
        await Js.FocusAsync(FindOption(Value)?.Id);
    }

    private async Task ClickOption(Option option)
    {
        if (option.Value is T value)
            await SetValueAsync(value);

        if (_menu is not null)
            await _menu.CloseAsync();

        await FocusAsync();
    }

    private Option? FindOption(T? value)
    {
        return _menu?.Options.FirstOrDefault(o => Equals(o.Value, value));
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        OnValueChanged();
    }

    /// <inheritdoc />
    protected override void OnValueChanged()
    {
        if (_menu is null)
            return;

        foreach (var option in _menu.Options)
        {
            var selected = Equals(Value, option.Value);
            if (option.Selected != selected)
            {
                option.Selected = selected;
                option.NotifyStateHasChanged();
            }
        }
    }
}