using Chaser.Data;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Humanizer;

namespace Chaser.Modules;

[Group("staff", "Moderators management")]
public sealed class StaffModule : ModuleBase
{
    private readonly ChaserDbContext _db;
    private readonly InteractiveService _interactive;

    public StaffModule(ChaserDbContext db, InteractiveService interactive)
    {
        _db = db;
        _interactive = interactive;
    }

    [RequireOwner]
    [SlashCommand("add", "Grant a user staff membership.")]
    public async Task<RuntimeResult> AddAsync([Summary(description: "The user to grant.")] IUser user)
    {
        if (_db.Moderators.Any(e => e.Id == user.Id))
            return InteractionResult.FromError($"{user.Mention} has already been added to the staff!");

        _db.Moderators.Add(new Moderator() { Id = user.Id });
        await _db.SaveChangesAsync();

        return InteractionResult.FromSuccess($"{user.Mention} has now been added to the staff!");
    }

    [RequireOwner]
    [SlashCommand("remove", "Remove staff privileges from a user.")]
    public async Task<RuntimeResult> RemoveAsync([Summary("user", "The moderator to be removed.")] Moderator moderator)
    {
        IUser user = await Context.Client.GetUserAsync(moderator.Id);

        _db.Moderators.Remove(moderator);
        await _db.SaveChangesAsync();

        return InteractionResult.FromSuccess($"{user.Mention} has now been removed from the staff!");
    }

    [SlashCommand("show", "Browse the entire list of all the moderators.")]
    public async Task ShowAsync()
    {
        var paginator = new DbPaginatorBuilder<Moderator>()
            .WithSet(_db.Moderators)
            .WithMaxPerPage(12)
            .AddPair(Emotes.Wrench, moderator =>
            {
                int identifiedScammersCount = _db.Scammers.Count(s => s.IdentityByUserId == moderator.Id);
                return typeof(Scammer).Name.ToLower().ToQuantity(identifiedScammersCount);
            })
            .AddPair(Emotes.Clock, m => TimestampTag.FromDateTimeOffset(m.IdentityAt, TimestampTagStyles.ShortDate))
            .Build();

        await _interactive.SendPaginatorAsync(paginator, Context.Interaction);
    }
}
