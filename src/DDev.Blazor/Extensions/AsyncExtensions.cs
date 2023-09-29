using System.Diagnostics.CodeAnalysis;

namespace DDev.Blazor.Extensions;

/// <summary>
/// Helper extensions for async operations.
/// </summary>
public static class AsyncExtensions
{
    /// <summary>
    /// Converts the sync <paramref name="action"/> to an async function.
    /// </summary>
    [return: NotNullIfNotNull(nameof(action))]
    internal static Func<Task>? ToAsync(this Action? action)
    {
        if (action is null)
            return null;

        return () =>
        {
            try
            {
                action();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        };
    }

    /// <summary>
    /// Converts the sync <paramref name="action"/> to an async function.
    /// </summary>
    [return: NotNullIfNotNull(nameof(action))]
    internal static Func<T, Task>? ToAsync<T>(this Action<T>? action)
    {
        if (action is null)
            return null;

        return value =>
        {
            try
            {
                action(value);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        };
    }

    /// <summary>
    /// Safely discards <paramref name="task"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="task"/> fails, the exception is sent to <paramref name="handleException"/> or ignored if <paramref name="handleException"/> is <see langword="null"/>.
    /// </remarks>
    public static async void Discard(this Task task, Action<Exception>? handleException)
    {
        try
        {
            await task;
        }
        catch (Exception exception)
        {
            handleException?.Invoke(exception);
        }
    }

    /// <summary>
    /// Safely discards <paramref name="task"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="task"/> fails, the exception is logged together with <paramref name="label"/>.
    /// </remarks>
    public static void Discard(this Task task, string label = "Task failed")
    {
        Discard(task, exception => Console.WriteLine($"{label}. {exception.GetType().Name} / {exception.Message}"));
    }
}
