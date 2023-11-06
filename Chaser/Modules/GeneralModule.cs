using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Reflection;

namespace Chaser.Modules;

public sealed class GeneralModule : ModuleBase
{
    private readonly LinksOptions _links;

    public GeneralModule(IOptions<LinksOptions> links)
    {
        _links = links.Value;
    }

    [SlashCommand("about", "Displays information about the application.")]
    public async Task AboutAsync()
    {
        await DeferAsync();

        string location = Assembly.GetExecutingAssembly().Location;
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(location);
        IApplication application = await Context.Client.GetApplicationInfoAsync();

        Embed embed = new EmbedBuilder()
            .WithTitle(application.Name)
            .WithDescription(application.Description)
            .AddField("Servers", Context.Client.Guilds.Count, true)
            .AddField("Latency", $"{Context.Client.Latency} ms", true)
            .AddField("Version", fileVersionInfo.ProductVersion, true)
            .WithThumbnailUrl(application.IconUrl)
            .WithColor(Colors.Primary)
            .Build();

        MessageComponent components = new ComponentBuilder()
            .WithButton("Vote", null, ButtonStyle.Link, Emotes.Logos.TopGG, _links.Vote)
            .WithButton("Join", null, ButtonStyle.Link, Emotes.Logos.Discord, _links.Discord)
            .WithButton("Contribute", null, ButtonStyle.Link, Emotes.Logos.GitHub, _links.Github)
            .Build();

        await FollowupAsync(embed: embed, components: components);
    }
}
