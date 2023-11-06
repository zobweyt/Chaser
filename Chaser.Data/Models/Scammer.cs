using Chaser.Core;
using System.ComponentModel.DataAnnotations;

namespace Chaser.Data;

/// <summary>
/// Represents a user who performed fraudulent activities.
/// </summary>
public class Scammer : DbEntity
{
    /// <summary>
    /// Gets or sets the reason why this scammer is banned.
    /// </summary>
    [MaxLength(RucoyConfig.MaxBanReasonLength)]
    public virtual string? BanReason { get; set; }

    /// <summary>
    /// Gets or sets the name of the character that this scammer owns.
    /// </summary>
    [MinLength(RucoyConfig.MinCharacterNameLength)]
    [MaxLength(RucoyConfig.MaxCharacterNameLength)]
    public virtual string? CharacterName { get; set; }

    /// <summary>
    /// Gets or sets the URL of the image showcasing the activities.
    /// </summary>
    [Url]
    public virtual string? ProofsImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that identified this scammer.
    /// </summary>
    public virtual ulong IdentityByUserId { get; set; }
}
