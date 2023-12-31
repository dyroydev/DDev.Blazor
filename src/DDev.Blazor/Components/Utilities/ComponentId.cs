﻿namespace DDev.Blazor.Components.Utilities;

/// <summary>
/// Utility for generating application-lifetime unique identities for components.
/// </summary>
public static class ComponentId
{
    private const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Length of ids generatied by <see cref="New"/>.
    /// </summary>
    public const int LENGTH = 6;

    /// <summary>
    /// Generates a new id with length <see cref="LENGTH"/>.
    /// </summary>
    public static string New()
    {
        var builder = new StringBuilder(LENGTH);

        for (var i = 0; i < LENGTH; i++)
            builder.Append(CHARACTERS[Random.Shared.Next(CHARACTERS.Length)]);

        return builder.ToString();
    }
}