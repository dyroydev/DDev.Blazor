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

    private class DisposableJsObjectReference : IDisposable, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsObject;
        private bool _disposed;

        public DisposableJsObjectReference(IJSObjectReference jsObject)
        {
            _jsObject = jsObject;
        }

        public async void Dispose()
        {
            try
            {
                await DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to dispose object: {ex.GetType().Name}/{ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                await _jsObject.InvokeVoidAsync("dispose");
            }
            finally
            {
                await _jsObject.DisposeAsync();
            }
        }
    }
}
