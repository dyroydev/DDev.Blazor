namespace DDev.Blazor.Services;

/// <summary>
/// Utility for generating css colors.
/// </summary>
public class CssColor
{
    /// <summary>
    /// Generates a random HSL value.
    /// </summary>
    /// <param name="seed">Seed used to make consistent colors.</param>
    /// <param name="saturation">Value range for saturation.</param>
    /// <param name="lightness">Value range for lightness.</param>
    /// <returns>A <see cref="string"/> formatted as a CSS hsl-color: <c>"hsl(hue, saturation%, lightness%)"</c>.</returns>
    public static string RandomHSL(object? seed, (int min, int max) saturation, (int min, int max) lightness)
    {
        var hash = Math.Abs(seed?.GetHashCode() ?? Random.Shared.Next());

        var hue = hash % 360;

        var saturationValue = (hash % (saturation.max - saturation.min)) + saturation.min;

        var lightnessValue = (hash % (lightness.max - lightness.min)) + lightness.min;

        return $"hsl({hue}, {saturationValue}%, {lightnessValue}%)";
    }

    /// <summary>
    /// Generates neutral washed colors.
    /// </summary>
    /// <remarks>
    /// Generates colors have a high contrast to <c>--on-surface</c> and a low contrast to <c>--on-primary</c>.
    /// </remarks>
    /// <param name="seed">Seed used to make consistent colors.</param>
    /// <returns>A <see cref="string"/> formatted as a CSS hsl-color: <c>"hsl(hue, saturation%, lightness%)"</c>.</returns>
    public static string RandomNeutral(object? seed) => RandomHSL(seed, saturation: (50, 75), lightness: (70, 90));

    /// <summary>
    /// Generates strong colors.
    /// </summary>
    /// <remarks>
    /// Generates colors have a high contrast to <c>--on-primary</c> and a low contrast to <c>--on-surface</c>.
    /// </remarks>
    /// <param name="seed">Seed used to make consistent colors.</param>
    /// <returns>A <see cref="string"/> formatted as a CSS hsl-color: <c>"hsl(hue, saturation%, lightness%)"</c>.</returns>
    public static string RandomStrong(object? seed) => RandomHSL(seed, saturation: (50, 35), lightness: (25, 60));
}