﻿using DDev.Blazor.Services;

namespace DDev.Blazor.Components.Communication;

internal class MessageBoxInternal : ComponentBase, IMessageBox
{
    private Dialog? _dialog;
    private string? _title;
    private string? _icon;
    private RenderFragment<Dialog>? _statement;
    private string? _confirm;
    private string? _confirmIcon;
    private string? _dismiss;
    private string? _dismissIcon;

    private Task<bool> ShowAsync(RenderFragment<Dialog> statement, string? title, string? icon, string? confirm, string? confirmIcon, string? dismiss, string? dismissIcon)
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
        builder.AddAttribute(2, nameof(Dialog.Dismissable), true);
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

    public Task AlertAsync(RenderFragment statement, string? title = null, string? icon = null, string confirm = "Ok", string? confirmIcon = null)
    {
        return ShowAsync(_ => statement, title, icon, confirm, confirmIcon, dismiss:null, dismissIcon:null);
    }

    public Task<bool> ConfirmAsync(RenderFragment statement, string? title = null, string? icon = null, string confirm = "Confirm", string? confirmIcon = null, string dismiss = "Dismiss", string? dismissIcon = null)
    {
        return ShowAsync(_ => statement, title, icon, confirm, confirmIcon, dismiss, dismissIcon);
    }
}