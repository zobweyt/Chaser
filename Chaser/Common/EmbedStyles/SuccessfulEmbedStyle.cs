using Discord;

namespace Chaser;

/// <summary>
/// Represents the style of a success embed.
/// </summary>
public class SuccessfulEmbedStyle : EmbedStyle
{
    public override string Name => "Succeed!";

    public override string IconUrl => Icons.Check;

    public override string? Footer => "Your changes have been successfully saved.";

    public override Color Color => Colors.Success;
}
