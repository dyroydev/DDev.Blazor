namespace DDev.Blazor.Components.Communication;

/// <summary>
/// The component responsible for displaying message boxes.
/// </summary>
internal sealed class MessageBoxProvider : ComponentBase
{
    private Dialog? _dialog;
    private string? _title;
    private string? _icon;
    private RenderFragment<Dialog>? _statement;
    private string? _confirm;
    private string? _confirmIcon;
    private string? _dismiss;
    private string? _dismissIcon;

    /// <summary>
    /// Shows the message box with the given parameters.
    /// </summary>
    /// <remarks>This method is thread safe.</remarks>
    /// <returns><see langword="true"/> if the user clicked the confirming action. Otherwise <see langword="false"/>.</returns>
    public async Task<bool> ShowAsyncThreadSafe(RenderFragment<Dialog> statement, string? title, string? icon, string? confirm, string? confirmIcon, string? dismiss, string? dismissIcon)
    {
        var result = false;
        await InvokeAsync(async () => result = await ShowAsync(statement, title, icon, confirm, confirmIcon, dismiss, dismissIcon));
        return result;
    }

    /// <summary>
    /// Shows the message box with the given parameters.
    /// </summary>
    /// <remarks>This method can ONLY be invoked on the UI-thread.</remarks>
    /// <returns><see langword="true"/> if the user clicked the confirming action. Otherwise <see langword="false"/>.</returns>
    public Task<bool> ShowAsync(RenderFragment<Dialog> statement, string? title, string? icon, string? confirm, string? confirmIcon, string? dismiss, string? dismissIcon)
    {
        _dialog?.Close();
        _title = title;
        _icon = icon;
        _confirm = confirm;
        _confirmIcon = confirmIcon;
        _dismiss = dismiss;
        _dismissIcon = dismissIcon;
        _statement = statement;

        StateHasChanged();
        return _dialog?.ShowAsync<bool>() ?? Task.FromResult(false);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<Dialog>(0);
        builder.AddAttribute(2, nameof(Dialog.Dismissible), true);
        builder.AddAttribute(3, nameof(Dialog.Content), _statement);

        if (string.IsNullOrWhiteSpace(_title) is false ||
            string.IsNullOrWhiteSpace(_icon) is false)
            builder.AddAttribute(4, nameof(Dialog.Header), (RenderFragment<Dialog>)BuildHeaderRenderTree);

        if (string.IsNullOrWhiteSpace(_confirm) is false ||
            string.IsNullOrWhiteSpace(_confirmIcon) is false ||
            string.IsNullOrWhiteSpace(_dismiss) is false ||
            string.IsNullOrWhiteSpace(_dismissIcon) is false)
            builder.AddAttribute(5, nameof(Dialog.Actions), (RenderFragment<Dialog>)BuildActionsRenderTree);

        builder.AddComponentReferenceCapture(1, dialog => _dialog = (Dialog?)dialog);
        builder.CloseComponent();
    }

    private RenderFragment BuildHeaderRenderTree(Dialog dialog) => builder =>
    {
        if (string.IsNullOrWhiteSpace(_icon) is false)
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, nameof(Icon.Name), _icon);
            builder.CloseComponent();
        }

        builder.AddContent(2, _title);
    };

    private RenderFragment BuildActionsRenderTree(Dialog dialog) => builder =>
    {
        if (string.IsNullOrWhiteSpace(_dismiss) is false ||
            string.IsNullOrWhiteSpace(_dismissIcon) is false)
        {
            builder.OpenComponent<Button>(0);
            builder.AddAttribute(1, nameof(Button.ChildContent), ButtonChildContentFragment(_dismiss, _dismissIcon));
            builder.AddAttribute(3, nameof(Button.OnClick), EventCallback.Factory.Create(this, HandleDismiss));
            builder.CloseComponent();
        }

        if (string.IsNullOrWhiteSpace(_confirm) is false ||
            string.IsNullOrWhiteSpace(_confirmIcon) is false)
        {
            builder.OpenComponent<Button>(4);
            builder.AddAttribute(5, nameof(Button.ChildContent), ButtonChildContentFragment(_confirm, _confirmIcon));
            builder.AddAttribute(6, nameof(Button.Primary), true);
            builder.AddAttribute(7, nameof(Button.OnClick), EventCallback.Factory.Create(this, HandleConfirm));
            builder.CloseComponent();
        }
    };

    private RenderFragment ButtonChildContentFragment(string? text, string? icon) => builder =>
    {
        if (string.IsNullOrWhiteSpace(icon) is false)
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, nameof(Icon.Name), icon);
            builder.CloseComponent();
        }

        if (string.IsNullOrWhiteSpace(text) is false)
        {
            builder.AddContent(2, text);
        }
    };

    private void HandleConfirm()
    {
        _dialog?.CloseWithResult(true);
    }

    private void HandleDismiss()
    {
        _dialog?.CloseWithResult(false);
    }
}