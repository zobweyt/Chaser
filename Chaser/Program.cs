using Chaser;
using Chaser.Services;
using Discord;
using Discord.Addons.Hosting;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddOptions<LinksOptions>().BindConfiguration(LinksOptions.Links);
        services.AddOptions<StartupOptions>().BindConfiguration(StartupOptions.Startup);
    })
    .ConfigureDiscordHost((context, config) =>
    {
        config.SocketConfig = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            LogGatewayIntentWarnings = false,
            UseInteractionSnowflakeDate = false,
            AlwaysDownloadUsers = true,
        };

        config.Token = context.Configuration.GetSection(StartupOptions.Startup).Get<StartupOptions>().Token;
    })
    .UseInteractionService((context, config) =>
    {
        config.LogLevel = LogSeverity.Debug;
        config.DefaultRunMode = RunMode.Async;
        config.UseCompiledLambda = false;
    })
    .UseInteractiveService((context, config) =>
    {
        config.LogLevel = LogSeverity.Critical;
        config.DefaultTimeout = TimeSpan.FromMinutes(5);
        config.ProcessSinglePagePaginators = true;
    })
    .ConfigureDatabase((context, options) =>
    {
        var connectionString = context.Configuration.GetConnectionString("Default");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        options.UseMySql(connectionString, serverVersion);
    })
    .ConfigureServices((context, services) =>
    {
        services
            .AddHttpClient()
            .AddScoped<BlacklistService>()
            .AddSingleton<InteractionRouteService>()
            .AddHostedService<InteractionHandlingService>()
            .AddHostedService<CustomStatusService>()
            .AddHostedService<ScammerTrackingService>();
    })
    .Build();

await host.MigrateAsync();
await host.RunAsync();
