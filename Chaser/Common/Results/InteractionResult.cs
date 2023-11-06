using Discord;
using Discord.Interactions;

namespace Chaser;

/// <summary>
/// Represents the result of an interaction command used to encapsulate the result of an
/// interaction command, including an error code and an optional message.
/// </summary>
public class InteractionResult : RuntimeResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionResult"/> class with the specified
    /// error and message.
    /// </summary>
    /// <param name="error">The error code to include in the result.</param>
    /// <param name="message">The message to include in the result.</param>
    private InteractionResult(InteractionCommandError? error, string message, InteractionResponseType? responseType)
        : base(error, message) 
    {
        ResponseType = responseType ?? InteractionResponseType.ChannelMessageWithSource;
    }

    public InteractionResponseType ResponseType { get; }

    /// <summary>
    /// Creates a successful interaction result with an optional message.
    /// </summary>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>
    /// A new instance of the <see cref="InteractionResult"/> with the specified message.
    /// </returns>
    public static InteractionResult FromSuccess(string? message = null, InteractionResponseType? responseType = null) 
        => new(null, message ?? string.Empty, responseType);

    /// <summary>
    /// Creates an unsuccessful interaction result with the specified error and message.
    /// </summary>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>
    /// A new instance of the <see cref="InteractionResult"/> with the
    /// <see cref="InteractionCommandError.Unsuccessful"/> code and message.
    /// </returns>
    public static InteractionResult FromError(string message, InteractionResponseType? responseType = null)
        => new(InteractionCommandError.Unsuccessful, message, responseType);
}
