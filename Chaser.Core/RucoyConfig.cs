namespace Chaser.Core;

/// <summary>
/// Defines various behaviors of the Rucoy Online.
/// </summary>
public static class RucoyConfig
{
    /// <summary>
    /// The base Rucoy Online website URL.
    /// </summary>
    public const string BaseUrl = "https://www.rucoyonline.com/";

    /// <summary>
    /// The minimum character name length allowed in the Rucoy Online.
    /// </summary>
    public const int MinCharacterNameLength = 3;

    /// <summary>
    /// The maximum character name length allowed in the Rucoy Online.
    /// </summary>
    public const int MaxCharacterNameLength = 15;
    
    /// <summary>
    /// The maximum number of characters allowed for a ban reason.
    /// </summary>
    public const int MaxBanReasonLength = 512;
}
