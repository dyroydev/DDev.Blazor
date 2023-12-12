namespace DDev.Blazor.Services;

internal class MessageBox(TaskCompletionSource<MessageBoxProvider> messageBoxTask) : IMessageBox
{
    public async Task AlertAsync(RenderFragment statement, string? title = null, string? icon = null, string? confirm = "Ok", string? confirmIcon = null)
    {
        var messageBox = await messageBoxTask.Task.ConfigureAwait(false);
        await messageBox.ShowAsyncThreadSafe(_ => statement, title, icon, confirm, confirmIcon, null, null).ConfigureAwait(false);
    }

    public async Task<bool> ConfirmAsync(RenderFragment statement, string? title = null, string? icon = null, string? confirm = "Confirm", string? confirmIcon = null, string? dismiss = "Dismiss", string? dismissIcon = null)
    {
        var messageBox = await messageBoxTask.Task.ConfigureAwait(false);
        return await messageBox.ShowAsyncThreadSafe(_ => statement, title, icon, confirm, confirmIcon, null, null).ConfigureAwait(false);
    }
}
