namespace DDev.Blazor.Services;

/// <summary>
/// Can show alert and confirm dialogs too the user.
/// </summary>
public interface IMessageBox
{
    /// <summary>
    /// Presents <paramref name="statement"/> to the the user with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed.</returns>
    public Task AlertAsync(string statement, string? title = null, string? icon = null, string? confirm = "Ok", string? confirmIcon = null)
        => AlertAsync(b => b.AddContent(0, statement), title, icon, confirm, confirmIcon);

    /// <summary>
    /// Presents <paramref name="statement"/> to the the user with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed.</returns>
    public Task AlertAsync(MarkupString statement, string? title = null, string? icon = null, string? confirm = "Ok", string? confirmIcon = null)
        => AlertAsync(b => b.AddContent(0, statement), title, icon, confirm, confirmIcon);

    /// <summary>
    /// Presents <paramref name="statement"/> to the the user with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed.</returns>
    public Task AlertAsync(RenderFragment statement, string? title = null, string? icon = null, string? confirm = "Ok", string? confirmIcon = null);

    /// <summary>
    /// Prompts the user to confirm or dismiss <paramref name="statement"/> with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <param name="dismiss">Text in the dismissing button.</param>
    /// <param name="dismissIcon">Icon in the dismissing button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed. The task contains <see cref="bool"/> value indicating wether <paramref name="statement"/> was confirmed or dismissed.</returns>
    public Task<bool> ConfirmAsync(string statement, string? title = null, string? icon = null, string? confirm = "Confirm", string? confirmIcon = null, string dismiss = "Dismiss", string? dismissIcon = null)
        => ConfirmAsync(b => b.AddContent(0, statement), title, icon, confirm, confirmIcon, dismiss, dismissIcon);

    /// <summary>
    /// Prompts the user to confirm or dismiss <paramref name="statement"/> with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <param name="dismiss">Text in the dismissing button.</param>
    /// <param name="dismissIcon">Icon in the dismissing button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed. The task contains <see cref="bool"/> value indicating wether <paramref name="statement"/> was confirmed or dismissed.</returns>
    public Task<bool> ConfirmAsync(MarkupString statement, string? title = null, string? icon = null, string? confirm = "Confirm", string? confirmIcon = null, string dismiss = "Dismiss", string? dismissIcon = null)
        => ConfirmAsync(b => b.AddContent(0, statement), title, icon, confirm, confirmIcon, dismiss, dismissIcon);

    /// <summary>
    /// Prompts the user to confirm or dismiss <paramref name="statement"/> with a <see cref="Dialog"/>.
    /// </summary>
    /// <param name="statement">Statement presented to user.</param>
    /// <param name="title">Title of dialog.</param>
    /// <param name="icon">Icon preceding <paramref name="title"/>.</param>
    /// <param name="confirm">Text in the confirming button.</param>
    /// <param name="confirmIcon">Icon in the confirming button.</param>
    /// <param name="dismiss">Text in the dismissing button.</param>
    /// <param name="dismissIcon">Icon in the dismissing button.</param>
    /// <returns>A <see cref="Task"/> completing when the dialog is closed. The task contains <see cref="bool"/> value indicating wether <paramref name="statement"/> was confirmed or dismissed.</returns>
    public Task<bool> ConfirmAsync(RenderFragment statement, string? title = null, string? icon = null, string? confirm = "Confirm", string? confirmIcon = null, string dismiss = "Dismiss", string? dismissIcon = null);
}