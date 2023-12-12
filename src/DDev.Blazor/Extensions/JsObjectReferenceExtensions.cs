namespace DDev.Blazor.Extensions;

/// <summary>
/// Useful extension methods for <see cref="IJSObjectReference"/>.
/// </summary>
internal static class JsObjectReferenceExtensions
{
    /// <summary>
    /// Returns a disposable wrapper for <paramref name="jsObject"/>. Assumes <paramref name="jsObject"/> has a <c>dispose()</c> method.
    /// </summary>
    public static IDisposable ToDisposable(this IJSObjectReference jsObject)
    {
        return new DisposableJsObjectReference(jsObject);
    }

    private class DisposableJsObjectReference(IJSObjectReference jsObject) : IDisposable, IAsyncDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            DisposeAsync().AsTask()
                .Discard(ex => Console.WriteLine($"Failed to dispose object: {ex.GetType().Name}/{ex.Message}"));
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                await jsObject.InvokeVoidAsync("dispose");
            }
            finally
            {
                await jsObject.DisposeAsync();
            }
        }
    }
}
