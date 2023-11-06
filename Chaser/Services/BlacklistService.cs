using Chaser.Data;
using Discord;
using Discord.WebSocket;

namespace Chaser;

/// <summary>
/// Represents a service used to manage blacklist.
/// </summary>
public sealed class BlacklistService
{
    private readonly DiscordSocketClient _client;
    private readonly ChaserDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlacklistService"/> class.
    /// </summary>
    /// <param name="client">The Discord socket client used by this service.</param>
    /// <param name="db">The database context used by this service.</param>
    public BlacklistService(DiscordSocketClient client, ChaserDbContext db)
    {
        _client = client;
        _db = db;
    }

    /// <summary>
    /// Adds a scammer to the blacklist.
    /// </summary>
    /// <param name="scammer">The scammer to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync(Scammer scammer)
    {
        _db.Add(scammer);
        await _db.SaveChangesAsync();

        IEnumerable<Task> tasks = _client.Guilds
            .SelectMany(g => g.Users)
            .Where(u => u.Id == scammer.Id)
            .Select(u => u.Guild.TryAddBanAsync(u));

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Removes a scammer from the blacklist.
    /// </summary>
    /// <param name="scammer">The scammer to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveAsync(Scammer scammer)
    {
        _db.Remove(scammer);
        await _db.SaveChangesAsync();

        IUser user = await _client.GetUserAsync(scammer.Id);

        await Task.WhenAll(_client.Guilds.Select(g => g.TryRemoveBanAsync(user)));
    }
}
