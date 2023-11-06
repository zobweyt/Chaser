using Discord;

namespace Chaser;

/// <summary>
/// Provides extension methods for <see cref="IDiscordInteraction"/>.
/// </summary>
public static class DiscordInteractionExtensions
{
    /// <summary>
    /// Handles the interaction by responding with the specified response type.
    /// </summary>
    /// <param name="interaction">The <see cref="IDiscordInteraction"/> instance.</param>
    /// <param name="responseType">The type for the response.</param>
    /// <param name="embed">The <see cref="Embed"/> to include in the response.</param>
    /// <param name="components">The <see cref="MessageComponent"/> to include in the response.</param>
    /// <param name="ephemeral">A boolean indicating whether the response should be ephemeral.</param>
    /// <returns>A task representing the asynchronous handling operation.</returns>
    public static async Task HandleAsync(this IDiscordInteraction interaction, InteractionResponseType responseType, Embed? embed = null, MessageComponent? components = null, bool ephemeral = false)
    {
        switch (responseType)
        {
            case InteractionResponseType.ChannelMessageWithSource:
                await interaction.RespondAsync(embed: embed, components: components, ephemeral: ephemeral).ConfigureAwait(false);
                break;

            case InteractionResponseType.DeferredChannelMessageWithSource:
                await interaction.FollowupAsync(embed: embed, components: components, ephemeral: ephemeral).ConfigureAwait(false);
                break;

            case InteractionResponseType.DeferredUpdateMessage:
                await interaction.ModifyOriginalResponseAsync(UpdateMessage).ConfigureAwait(false);
                break;

            case InteractionResponseType.UpdateMessage:
                await ((IComponentInteraction)interaction).UpdateAsync(UpdateMessage).ConfigureAwait(false);
                break;

            case InteractionResponseType.Modal:
                await ((IModalInteraction)interaction).UpdateAsync(UpdateMessage).ConfigureAwait(false);
                break;

            default:
                throw new ArgumentException("Unsupported interaction response type.", nameof(responseType));
        }

        void UpdateMessage(MessageProperties props)
        {
            props.Embed = embed;
            props.Components = components;
        }
    }
}
