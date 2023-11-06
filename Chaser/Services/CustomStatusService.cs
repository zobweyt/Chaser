using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;
using Fergun.Interactive;

namespace Chaser.Services;

internal sealed class CustomStatusService : DiscordClientService
{
    private readonly string[] _quotes =
    {
        "Hosting a beginners' training session!",
        "Chilling in the town square, feeling nostalgic…",
    };

    private readonly Random _random = new();
    private readonly InteractiveConfig _config;

    public CustomStatusService(DiscordSocketClient client, ILogger<DiscordClientService> logger, InteractiveConfig config)
        : base(client, logger) 
    {
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Client.WaitForReadyAsync(stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Client.SetCustomStatusAsync(_quotes[_random.Next(0, _quotes.Length)]);
            await Task.Delay(_config.DefaultTimeout, stoppingToken);
        }
    }
}
