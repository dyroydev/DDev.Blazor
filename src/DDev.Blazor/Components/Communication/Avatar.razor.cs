namespace DDev.Blazor.Components.Communication;

/// <summary>
/// An avatar shows an image or text to represent a person or group as well as give additional information like their status and activity.
/// </summary>
public partial class Avatar
{
    /// <summary>
    /// Id of the item represented by the avatar. (Used to pick consistent color).
    /// </summary>
    [Parameter] public object? Id { get; set; }

    /// <summary>
    /// Name of the item represented by the avarat. (Used to make tooltip and initials).
    /// </summary>
    [Parameter] public string? Name { get; set; }

    /// <summary>
    /// Initials visible on the avatar. (Should contain no more than three characters).
    /// </summary>
    [Parameter] public string? Initials { get; set; }

    /// <summary>
    /// Optional URL to image representing the item.
    /// </summary>
    [Parameter] public string? ImageUrl { get; set; }

    /// <summary>
    /// Optional override for background color. Value is a CSS color.
    /// </summary>
    [Parameter] public string? Color { get; set; }

    /// <summary>
    /// Optional override for size of avatar. Value must include a CSS unit.
    /// </summary>
    [Parameter] public string? Size { get; set; }

    /// <summary>
    /// Optional override for tooltip shown when hovering avatar. (Default to Name).
    /// </summary>
    [Parameter] public string? Tooltip { get; set; }

    /// <summary>
    /// Optional icon indicating the status of the represented item.
    /// </summary>
    [Parameter] public string? StatusIcon { get; set; }

    /// <summary>
    /// Optional color indicating the status of the represented item.
    /// </summary>
    [Parameter] public string? StatusColor { get; set; }

    /// <summary>
    /// Optional tooltip informing the status of the represented item.
    /// </summary>
    [Parameter] public string? StatusTooltip { get; set; }

    /// <summary>
    /// Callback invoked when the avatar is clicked.
    /// </summary>
    [Parameter] public EventCallback OnClick { get; set; }

    private string? _initials;
    private string? _color;
    private string? _size;
    private string? _tooltip;
    private bool _hasStatus;

    protected override void OnParametersSet()
    {
        _color = string.IsNullOrWhiteSpace(Color)
            ? GenerateBackgroundColor(Id ?? Name ?? Initials)
            : Color;

        _initials = string.IsNullOrWhiteSpace(Initials)
            ? GenerateInitials(Name ?? "", 2)
            : Initials;

        _size = string.IsNullOrWhiteSpace(Size)
            ? "40px"
            : Size;

        _tooltip = Tooltip ?? Name;

        _hasStatus = string.IsNullOrWhiteSpace(StatusIcon) is false ||
                string.IsNullOrWhiteSpace(StatusTooltip) is false ||
                string.IsNullOrWhiteSpace(StatusColor) is false;
    }

    /// <summary>
    /// Generates a random color consistent for the given <paramref name="seed"/>.
    /// </summary>
    /// <returns>A CSS color value.</returns>
    public static string GenerateBackgroundColor(object? seed)
    {
        var hash = Math.Abs(seed?.GetHashCode() ?? 0);

        // Range: 0 - 360
        var hue = hash % 360;

        // Range: 50 - 75
        var saturation = (hash % 25) + 50;

        // Range: 25 - 60
        var lightness = (hash % 35) + 25;

        return $"hsl({hue}, {saturation}%, {lightness}%)";
    }

    /// <summary>
    /// Generates initials from the name.
    /// </summary>
    /// <param name="name">Name of represented item.</param>
    /// <param name="maxLength">Maximum characters in initials.</param>
    public static string GenerateInitials(string name, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(name) || maxLength < 1)
            return "";

        var parts = name.Split(' ');

        var length = Math.Min(maxLength, parts.Length);

        var sb = new StringBuilder(length);
        sb.Append(char.ToUpperInvariant(parts[0][0]));

        for (var i = 1; i < length - 1; i++)
            sb.Append(char.ToUpperInvariant(parts[i][0]));

        if (length > 1)
            sb.Append(char.ToUpperInvariant(parts[^1][0]));

        return sb.ToString();
    }
}
