using Discord;

namespace Chaser;

/// <summary>
/// Represents a constant set of predefined <see cref="Color"/> values.
/// </summary>
public static class Colors
{
    /// <summary>
    /// The color used to indicate an informative state.
    /// </summary>
    public static readonly Color Primary = new(51, 122, 183);

    /// <summary>
    /// The color used to indicate an emotion of positivity.
    /// </summary>
    public static readonly Color Success = new(35, 137, 69);

    /// <summary>
    /// The color used to indicate an emotion of negativity.
    /// </summary>
    public static readonly Color Danger = new(172, 38, 38);
}
