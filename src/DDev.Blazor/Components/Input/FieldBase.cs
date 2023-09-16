using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace DDev.Blazor.Components.Input;

/// <summary>
/// Base class for input field components.<br/>
/// This components integrates with <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/> which is optionally supplied by a cascading parameter.
/// </summary>
public abstract class FieldBase<T> : ComponentBase, IDisposable
{
    /// <summary>
    /// Value for id-attribute on the field element.
    /// </summary>
    [Parameter] public string Id { get; set; } = ComponentId.New();

    /// <summary>
    /// Label describing the field. Should be short.
    /// </summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the field is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; }

    /// <summary>
    /// Current value of field.
    /// </summary>
    #pragma warning disable BL0007
    [Parameter] public T? Value { get => _value; set => SetValueAsync(value).Discard("Failed to set Value"); }
    #pragma warning restore BL0007

    /// <summary>
    /// Callback invoked after <see cref="Value"/> has changed.
    /// </summary>
    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    /// <summary>
    /// Optional expression used to identify which validation applies to the field.
    /// </summary>
    [Parameter] public Expression<Func<T?>>? ValueExpression { get; set; }

    /// <summary>
    /// <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/> currently validating this field.
    /// </summary>
    [CascadingParameter] protected EditContext? EditContext { get => _editContext; private set => SetEditContext(value); }

    /// <summary>
    /// JS runtime associated with this component.
    /// </summary>
    [Inject] protected IJSRuntime Js { get; set; } = null!;

    /// <summary>
    /// <see langword="true"/> if field is validated and field was not valid.
    /// </summary>
    /// <remarks>Always <see langword="false"/> if <see cref="EditContext"/> is <see langword="null"/>.</remarks>
    protected bool IsInvalid { get; private set; }

    private EditContext? _editContext;
    private T? _value;


    private void SetEditContext(EditContext? editContext)
    {
        if (_editContext == editContext)
            return;

        if (_editContext is not null)
            _editContext.OnFieldChanged -= HandleValidationStateChanged;

        _editContext = editContext;

        if (_editContext is not null)
            _editContext.OnFieldChanged += HandleValidationStateChanged;
    }

    private void HandleValidationStateChanged(object? sender, FieldChangedEventArgs args)
    {
        if (_editContext is null || ValueExpression is null)
            return;

        var fieldId = FieldIdentifier.Create(ValueExpression);
        var isInvalid = _editContext.GetValidationMessages(fieldId).Any();
        if (isInvalid == IsInvalid)
            return;

        IsInvalid = isInvalid;
        OnInvalidChanged();
        OnInvalidChangedAsync().Discard($"Failed to notify {nameof(OnInvalidChanged)}");
    }

    /// <summary>
    /// Delegates focus to this field.
    /// </summary>
    public virtual async Task FocusAsync()
    {
        await Js.FocusAsync(Id);
    }

    /// <summary>
    /// Sets <see cref="Value"/> to the given <paramref name="value"/>.
    /// </summary>
    /// <remarks>
    /// If you override this, make sure to invoke and await <c>base.SetValueAsync(value)</c>.
    /// </remarks>
    /// <returns>A <see cref="Task"/> that completes when value is set and all lifetime methods have completed.</returns>
    protected virtual async Task SetValueAsync(T? value)
    {
        if (Equals(value, _value))
            return;

        _value = value;
        OnValueChanged();
        await (OnValueChangedAsync() ?? Task.CompletedTask);
        await ValueChanged.InvokeAsync(_value);
    }

    /// <summary>
    /// Releases any resources held by this field.
    /// </summary>
    public void Dispose()
    {
        SetEditContext(null);
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Should release any resources held by this field.
    /// </summary>
    /// <param name="disposing">Always <see langword="true"/>.</param>
    protected virtual void Dispose(bool disposing) { }

    /// <summary>
    /// Callback invoked after <see cref="Value"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual void OnValueChanged() { }

    /// <summary>
    /// Callback invoked after <see cref="Value"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual Task OnValueChangedAsync() => Task.CompletedTask;

    /// <summary>
    /// Callback invoked after <see cref="IsInvalid"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual void OnInvalidChanged() { }

    /// <summary>
    /// Callback invoked after <see cref="IsInvalid"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual Task OnInvalidChangedAsync() => Task.CompletedTask;
}