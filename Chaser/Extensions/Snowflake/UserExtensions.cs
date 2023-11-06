using Discord;

namespace Chaser;

/// <summary>
/// Provides extension methods for <see cref="IUser"/>.
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// Gets avatar URL or the default one.
    /// </summary>
    /// <param name="user">The user to get avatar URL.</param>
    /// <returns>The avatar URL.</returns>
    public static string GetDisplayAvatarUrl(this IUser user) => user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();

    /// <summary>
    /// Determines if the current user has a higher hierarchy than the target user in a guild.
    /// </summary>
    /// <param name="user">The user to compare.</param>
    /// <param name="targetUser">The target to compare hierarchy with.</param>
    /// <returns>
    /// <see langword="true"/> if the current user has a higher hierarchy than
    /// the target user, otherwise <see langword="false"/>.
    /// </returns>
    public static bool HasHigherHierarchy(this IGuildUser user, IUser targetUser)
        => ((targetUser as IGuildUser)?.Hierarchy ?? int.MinValue) < user.Hierarchy;
}
