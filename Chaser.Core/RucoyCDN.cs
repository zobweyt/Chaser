namespace Chaser.Core;

/// <summary>
/// Contains strings related to the Rucoy Online's content delivery networks.
/// </summary>
public static class RucoyCDN
{
    /// <summary>
    /// Gets a character URL based on its name.
    /// </summary>
    /// <param name="name">The name of the character.</param>
    /// <returns>A URL pointing to the character.</returns>
    public static string GetCharacterUrl(string name) => $"{RucoyConfig.BaseUrl}characters/{Uri.EscapeDataString(name)}";
}
