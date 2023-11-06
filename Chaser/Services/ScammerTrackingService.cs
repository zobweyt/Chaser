using Chaser.Data;
using Discord.Addons.Hosting;
using Discord.WebSocket;

namespace Chaser;

/// <summary>
/// Service class for tracking scammers and applying bans when necessary.
/// </summary>
internal sealed class ScammerTrackingService : DiscordClientService
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScammerTrackingService"/> class.
    /// </summary>
    /// <param name="client">The Discord socket client used by this service.</param>
    /// <param name="logger">The logger used by this service.</param>
    /// <param name="provider">The service provider used by this service.</param>
    public ScammerTrackingService(DiscordSocketClient client, ILogger<DiscordClientService> logger, IServiceProvider provider)
        : base(client, logger)
    {
        _provider = provider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.UserJoined += OnUserJoinedAsync;
        Client.JoinedGuild += OnJoinedGuildAsync;

        return Task.CompletedTask;
    }

    private async Task OnUserJoinedAsync(SocketGuildUser user)
    {
        await using var scope = _provider.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();

        Scammer? scammer = await db.Scammers.FindAsync(user.Id);

        if (scammer == null)
            return;

        await user.Guild.TryAddBanAsync(user);
    }

    private async Task OnJoinedGuildAsync(SocketGuild guild)
    {
        await using var scope = _provider.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();

        if (!guild.CurrentUser.GuildPermissions.BanMembers)
            return;

        HashSet<ulong> scammerIds = new(db.Scammers.Select(s => s.Id));
        await Task.WhenAll(guild.Users.Where(u => scammerIds.Contains(u.Id)).Select(guild.TryAddBanAsync));
    }
}
