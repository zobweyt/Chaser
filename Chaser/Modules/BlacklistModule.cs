using Chaser.Data;
using Chaser.Core;
using Chaser.Services;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;

namespace Chaser.Modules;

[Group("blacklist", "Scammers management")]
public sealed class BlacklistModule : ModuleBase
{
    private readonly ChaserDbContext _db;
    private readonly InteractionRouteService _route;
    private readonly InteractiveService _interactive;
    private readonly BlacklistService _blacklist;

    public BlacklistModule(ChaserDbContext db, InteractionRouteService route, InteractiveService interactive, BlacklistService blacklist)
    {
        _db = db;
        _route = route;
        _interactive = interactive;
        _blacklist = blacklist;
    }

    [RequireModerator]
    [SlashCommand("add", "Include a user on the list and enforce a ban on each accessible server.")]
    public async Task AddAsync() => await RespondWithModalAsync<ScammerModal>(_route.Bind(AddModalSubmitAsync));

    [RateLimit(max: 5, span: 1, TimeMeasure.Hours)]
    [ModalInteraction("BlacklistAddModalSubmit", true)]
    public async Task AddModalSubmitAsync([Validate] ScammerModal modal)
    {
        await DeferAsync();

        Scammer scammer = new()
        {
            Id = modal.Id,
            BanReason = modal.BanReason,
            CharacterName = modal.CharacterName,
            ProofsImageUrl = modal.ProofsImageUrl,
            IdentityByUserId = Context.User.Id,
        };

        await _blacklist.AddAsync(scammer);
        await InfoAsync(scammer, InteractionResponseType.DeferredChannelMessageWithSource);
    }

    [RequireModerator]
    [RateLimit(max: 2, span: 1, TimeMeasure.Hours)]
    [SlashCommand("remove", "Revoke a scammer from the list and remove the ban on each accessible server.")]
    public async Task<RuntimeResult> RemoveAsync([Summary("user", "The scammer to be removed.")] Scammer scammer)
    {
        await DeferAsync();

        await _blacklist.RemoveAsync(scammer);

        string message = $"This scammer has been removed and unbanned on each accessible server.";
        return InteractionResult.FromSuccess(message, InteractionResponseType.DeferredChannelMessageWithSource);
    }

    [SlashCommand("info", "Retrieve more information about any scammer on the list.")]
    public async Task InfoAsync([Summary("user", "The scammer to be shown.")] Scammer scammer)
        => await InfoAsync(scammer, InteractionResponseType.ChannelMessageWithSource);

    [RequireModerator]
    [ComponentInteraction("blacklist_edit:*", true)]
    public async Task EditAsync(Scammer scammer)
    {
        await RespondWithModalAsync<ScammerModal>(_route.Bind(EditModalSubmitAsync, scammer.Id), builder =>
        {
            builder.WithTitle("Editing scammer properties…");
            builder.UpdateTextInput(nameof(ScammerModal.BanReason), scammer.BanReason);
            builder.UpdateTextInput(nameof(ScammerModal.CharacterName), scammer.CharacterName);
            builder.UpdateTextInput(nameof(ScammerModal.ProofsImageUrl), scammer.ProofsImageUrl);
            builder.RemoveComponent(nameof(ScammerModal.Id));
        });
    }

    [RateLimit(max: 3, span: 1, TimeMeasure.Hours)]
    [ModalInteraction("blacklist_edit_modal_submit:*", true)]
    public async Task EditModalSubmitAsync(Scammer scammer, [Validate] ScammerModal modal)
    {
        scammer.BanReason = modal.BanReason;
        scammer.CharacterName = modal.CharacterName;
        scammer.ProofsImageUrl = modal.ProofsImageUrl;

        _db.Scammers.Update(scammer);
        await _db.SaveChangesAsync();

        await InfoAsync(scammer, InteractionResponseType.Modal);
    }

    [SlashCommand("show", "Browse the entire list of all the scammers.")]
    public async Task ShowAsync()
    {
        var paginator = new DbPaginatorBuilder<Scammer>()
            .WithSet(_db.Scammers)
            .WithMaxPerPage(12)
            .AddPair(Emotes.Character, scammer =>
            {
                if (string.IsNullOrWhiteSpace(scammer.CharacterName))
                    return Format.Strikethrough("Unknown");
                string url = RucoyCDN.GetCharacterUrl(scammer.CharacterName);
                return Format.Bold(Format.Url(scammer.CharacterName, url));
            })
            .AddPair(Emotes.File, scammer =>
            {
                if (string.IsNullOrWhiteSpace(scammer.ProofsImageUrl))
                    return Format.Strikethrough("Unspecified");
                return Format.Underline(Format.Url("View proofs", scammer.ProofsImageUrl));
            })
            .AddPair(Emotes.Clock, s => TimestampTag.FromDateTimeOffset(s.IdentityAt, TimestampTagStyles.ShortDate))
            .Build();

        await _interactive.SendPaginatorAsync(paginator, Context.Interaction);
    }

    private async Task InfoAsync(Scammer scammer, InteractionResponseType responseType)
    {
        IUser user = await Context.Client.GetUserAsync(scammer.Id);
        IUser author = await Context.Client.GetUserAsync(scammer.IdentityByUserId);

        Embed embed = new EmbedBuilder()
            .WithAuthor(user.ToString(), user.GetDisplayAvatarUrl())
            .WithDescription(scammer.BanReason)
            .WithImageUrl(scammer.ProofsImageUrl)
            .WithFooter(author.ToString(), author.GetDisplayAvatarUrl())
            .WithTimestamp(scammer.IdentityAt)
            .WithColor(Colors.Primary)
            .Build();

        ComponentBuilder componentBuilder = new ComponentBuilder()
            .WithButton("Edit", _route.Bind(EditAsync, scammer.Id), ButtonStyle.Primary, Emotes.Wrench);

        string? name = scammer.CharacterName;
        if (!string.IsNullOrWhiteSpace(name))
        {
            string url = RucoyCDN.GetCharacterUrl(name);
            componentBuilder.WithButton(name, null, ButtonStyle.Link, Emotes.Character, url);
        }

        await HandleAsync(responseType, embed, componentBuilder.Build());
    }
}
