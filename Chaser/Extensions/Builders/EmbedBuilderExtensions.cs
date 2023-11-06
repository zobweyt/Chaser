using Discord;

namespace Chaser;

/// <summary>
/// Provides extension methods for <see cref="EmbedBuilder"/>.
/// </summary>
public static class EmbedBuilderExtensions
{
    /// <summary>
    /// Applies an <see cref="EmbedStyle"/> for the current embed builder.
    /// </summary>
    /// <param name="builder">The current builder.</param>
    /// <param name="style">An <see cref="EmbedStyle"/> to apply.</param>
    /// <param name="name">The <see cref="EmbedStyle.Name"/> or style name.</param>
    /// <returns>The current builder instance with the style applied.</returns>
    public static EmbedBuilder WithStyle(this EmbedBuilder builder, EmbedStyle style)
    {
        builder
            .WithAuthor(author =>
            {
                author.WithName(style.Name).WithIconUrl(style.IconUrl);
            })
            .WithFooter(style.Footer)
            .WithColor(style.Color);

        return builder;
    }
}
