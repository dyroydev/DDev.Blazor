using DDev.Blazor.Internal;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DDev.Blazor.Components.Input.Custom;

/// <summary>
/// Base class for input field components.<br/>
/// This components integrates with <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/> which is optionally supplied by a cascading parameter.
/// </summary>
[DebuggerDisplay($"{{{nameof(Label)}}} {{GetType().Name}}")]
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
    [Parameter] public T? Value
    {
        get => _value; 
        set => InvokeSafe(() => SetValueAsync(value)); }
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
    protected bool IsFieldInvalid { get; private set; }

    private readonly ComposedDisposable _disposable = new();
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
        if (isInvalid == IsFieldInvalid)
            return;

        IsFieldInvalid = isInvalid;
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
    /// If <paramref name="utility"/> implements <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>, it will be disposed when this component is disposed.
    /// </summary>
    /// <typeparam name="TUtility">Type of utility service.</typeparam>
    /// <param name="utility">The utility owned by the component.</param>
    protected TUtility Use<TUtility>(TUtility utility)
    {
        _disposable.AddIfDisposable(utility);
        return utility;
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
        StateHasChanged();
        await ValueChanged.InvokeAsync(_value);
    }

    /// <summary>
    /// Immediately invokes <paramref name="action"/> and delegates exceptions to the components dispatcher.
    /// </summary>
    /// <remarks>This method assumes its being invoked from the components dispatcher.</remarks>
    /// <param name="action"></param>
    private async void InvokeSafe(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception exception)
        {
            await DispatchExceptionAsync(exception);
        }
    }

    /// <summary>
    /// Releases any resources held by this field.
    /// </summary>
    public void Dispose()
    {
        SetEditContext(null);
        _disposable.Dispose();
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
    /// Callback invoked after <see cref="IsFieldInvalid"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual void OnInvalidChanged() { }

    /// <summary>
    /// Callback invoked after <see cref="IsFieldInvalid"/> has changed.
    /// </summary>
    /// <remarks>Component is not automatically re-rendered after this callback.</remarks>
    protected virtual Task OnInvalidChangedAsync() => Task.CompletedTask;
}