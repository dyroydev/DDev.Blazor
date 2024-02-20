using System.ComponentModel;

namespace DDev.Blazor.Components.Layouts;

/// <summary>
/// Utility component for working with media queries.
/// </summary>
/// <remarks>
/// Instances (or <see langword="null"/>) can be converted to a boolean. <see langword="true"/> when not <see langword="null"/> the media matches, otherwise <see langword="false"/>.
/// </remarks>
public sealed class MediaQuery : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Any valid media query.
    /// </summary>
    [Parameter, EditorRequired] public string Match { get; set; } = "";

    /// <summary>
    /// Callback invoked when the media query matches or un-matches.
    /// </summary>
    [Parameter] public EventCallback<bool> Changed { get; set; }

    /// <summary>
    /// Content to render when the media query matches.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// If <see langword="true"/>, the media query is inverted.
    /// </summary>
    [Parameter] public bool Not { get; set; }
    
    /// <summary>
    /// If <see langword="true"/>, the media query matches. Default is <see langword="false"/>.
    /// </summary>
    public bool Matches { get; private set; }

    [Inject] private IJSRuntime Js { get; set; } = null!;

    private DotNetObjectReference<MediaQuery>? _thisReference;
    private IJSObjectReference? _mediaQueryReference;


    /// <summary>
    /// Disposes the media query.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_mediaQueryReference is not null)
        {
            await using (_mediaQueryReference)
            {
                await _mediaQueryReference.InvokeVoidAsync("dispose");
            }
            _mediaQueryReference = null;
        }

        if (_thisReference is not null)
        {
            using (_thisReference) { }
            _thisReference = null;
        }
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _thisReference = DotNetObjectReference.Create(this);
        _mediaQueryReference = await Js.InvokeDDevAsync<IJSObjectReference>("media-query", "createMediaQuery", Match, _thisReference);
    }

    /// <summary>
    /// Sets the media query to match or un-match.
    /// </summary>
    /// <remarks>Do not invoke manually. Invoked by JS.</remarks>
    [JSInvokable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public async Task SetMatchesAsync(bool matches)
    {
        if (Not) matches = !matches;
        if (matches == Matches)
            return;

        Matches = matches;
        await Changed.InvokeAsync(matches);
    }

    /// <summary>
    /// Converts the media query to a boolean value.
    /// </summary>
    public static implicit operator bool(MediaQuery? query) => query is { Matches: true };
}
