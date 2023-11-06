using Chaser.Data;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Chaser.Services;

/// <summary>
/// Represents a service used to handle interactions in this application.
/// </summary>
internal sealed class InteractionHandlingService : DiscordClientService
{
    private readonly IServiceProvider _provider;
    private readonly InteractionService _service;
    private readonly IHostEnvironment _environment;
    private readonly ulong _developmentGuildId;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionHandlingService"/>.
    /// </summary>
    /// <param name="client">The Discord socket client used by this service.</param>
    /// <param name="logger">The logger used by this service.</param>
    /// <param name="provider">The service provider used by this service.</param>
    /// <param name="service">The interaction service used by this service.</param>
    /// <param name="environment">The host environment used by this service.</param>
    /// <param name="configuration">The configuration used by this service.</param>
    public InteractionHandlingService(DiscordSocketClient client, ILogger<DiscordClientService> logger,
        IServiceProvider provider, InteractionService service, IHostEnvironment environment, IOptions<StartupOptions> options)
        : base(client, logger)
    {
        _provider = provider;
        _service = service;
        _environment = environment;
        _developmentGuildId = options.Value.DevelopmentGuildId;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _service.AddGenericTypeConverter<DbEntity>(typeof(EntityTypeConverter<>));
        _service.AddGenericTypeReader<DbEntity>(typeof(EntityTypeReader<>));

        await using var scope = _provider.CreateAsyncScope();
        await _service.AddModulesAsync(Assembly.GetEntryAssembly(), scope.ServiceProvider);

        Client.InteractionCreated += OnInteractionCreatedAsync;
        _service.InteractionExecuted += OnInteractionExecutedAsync;

        await Client.WaitForReadyAsync(stoppingToken);
        await RegisterCommandsAsync();
    }

    private async Task RegisterCommandsAsync()
    {
        if (_environment.IsDevelopment())
        {
            await Client.Rest.DeleteAllGlobalCommandsAsync();
            await _service.RegisterCommandsToGuildAsync(_developmentGuildId);
            return;
        }

        if (_developmentGuildId != 0)
            await Client.Rest.BulkOverwriteGuildCommands(Array.Empty<ApplicationCommandProperties>(), _developmentGuildId);
        else
            Logger.LogWarning("Potential duplication of application commands detected.");

        await _service.RegisterCommandsGloballyAsync();
    }

    private async Task OnInteractionCreatedAsync(SocketInteraction interaction)
    {
        try
        {
            SocketInteractionContext context = new(Client, interaction);
            await _service.ExecuteCommandAsync(context, _provider);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Exception occurred whilst attempting to handle interaction.");
        }
    }

    private async Task OnInteractionExecutedAsync(ICommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (string.IsNullOrWhiteSpace(result.ErrorReason) || result.Error == InteractionCommandError.UnknownCommand)
            return;

        Embed embed = new EmbedBuilder()
            .WithStyle(result.IsSuccess ? new SuccessfulEmbedStyle() : new UnsuccessfulEmbedStyle())
            .WithDescription(result.ErrorReason)
            .Build();
        
        InteractionResponseType responseType = (result as InteractionResult)?.ResponseType ?? InteractionResponseType.ChannelMessageWithSource;

        await context.Interaction.HandleAsync(responseType, embed, ephemeral: !result.IsSuccess);
    }
}
