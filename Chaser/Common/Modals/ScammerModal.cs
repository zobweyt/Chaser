using Chaser.Core;
using Chaser.Data;
using Discord;
using Discord.Interactions;

namespace Chaser;

/// <summary>
/// Represents a modal for <see cref="Scammer"/>.
/// </summary>
public class ScammerModal : IModal
{
    public virtual string Title => "Add a scammer";

    /// <summary>
    /// Gets or sets the new <see cref="Scammer.Id"/>.
    /// </summary>
    [User]
    [InputLabel("User ID")]
    [ModalTextInput(nameof(Id), placeholder: "9223372036854775807")]
    public virtual ulong Id { get; set; }

    /// <summary>
    /// Gets or sets the new <see cref="Scammer.BanReason"/>.
    /// </summary>
    [RequiredInput(false)]
    [InputLabel("Ban Reason (Markdown)")]
    [ModalTextInput(nameof(BanReason), TextInputStyle.Paragraph, "Describe the activities done by this person…", maxLength: RucoyConfig.MaxBanReasonLength)]
    public virtual string? BanReason { get; set; }

    /// <summary>
    /// Gets or sets the new <see cref="Scammer.CharacterName"/>.
    /// </summary>
    [RequiredInput(false)]
    [Name]
    [InputLabel("Character Name")]
    [ModalTextInput(nameof(CharacterName), placeholder: "Brolly Horde", minLength: RucoyConfig.MinCharacterNameLength, maxLength: RucoyConfig.MaxCharacterNameLength)]
    public virtual string? CharacterName { get; set; }

    /// <summary>
    /// Gets or sets the new <see cref="Scammer.ProofsImageUrl"/>.
    /// </summary>
    [RequiredInput(false)]
    [Image]
    [InputLabel("Proofs Image URL")]
    [ModalTextInput(nameof(ProofsImageUrl), placeholder: "https://i.imgur.com/example.png")]
    public virtual string? ProofsImageUrl { get; set; }
}
