using Chaser.Data;
using Discord;
using Discord.Interactions;

namespace Chaser;

/// <summary>
/// Specifies that an application command can be run only by a moderator user.
/// </summary>
public sealed class RequireModeratorAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();

        if (db.Moderators.Any(m => m.Id == context.User.Id))
            return Task.FromResult(PreconditionResult.FromSuccess());

        return Task.FromResult(PreconditionResult.FromError("This command can not be run with your permissions!"));
    }
}
