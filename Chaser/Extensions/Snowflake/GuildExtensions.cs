using Discord;

namespace Chaser;

/// <summary>
/// Provides extension methods for <see cref="IGuild"/>.
/// </summary>
public static class GuildExtensions
{
    /// <summary>
    /// Tries to add a ban for a user in a guild.
    /// </summary>
    /// <param name="guild">The guild to modify.</param>
    /// <param name="user">The user to add the ban for.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a boolean value indicating whether the ban status was added.
    /// </returns>
    public static Task<bool> TryAddBanAsync(this IGuild guild, IUser user) =>
        TryModifyBanAsync(guild, user, ban => ban != null, (guild, user) => guild.AddBanAsync(user));

    /// <summary>
    /// Tries to remove the ban of a user in a guild.
    /// </summary>
    /// <param name="guild">The guild to modify.</param>
    /// <param name="user">The user to remove the ban for.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a boolean value indicating whether the ban status was removed.
    /// </returns>
    public static Task<bool> TryRemoveBanAsync(this IGuild guild, IUser user) =>
        TryModifyBanAsync(guild, user, ban => ban == null, (guild, user) => guild.RemoveBanAsync(user));

    private static async Task<bool> TryModifyBanAsync(IGuild guild, IUser target, Func<IBan?, bool> predicate, Func<IGuild, IUser, Task> action)
    {
        IGuildUser? currentGuildUser = await guild.GetCurrentUserAsync().ConfigureAwait(false);

        if (currentGuildUser?.GuildPermissions.BanMembers != true)
            return false;

        IBan? ban = await guild.GetBanAsync(target).ConfigureAwait(false);

        if (predicate(ban))
            return false;

        IGuildUser? targetGuildUser = await guild.GetUserAsync(target.Id).ConfigureAwait(false);

        if (currentGuildUser?.HasHigherHierarchy(targetGuildUser) != true)
            return false;

        await action(guild, target);
        return true;
    }
}
