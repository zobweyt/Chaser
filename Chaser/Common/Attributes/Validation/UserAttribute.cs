using Chaser.Data;
using Discord;
using Discord.Interactions;
using System.Reflection;

namespace Chaser;

/// <summary>
/// Represents an attribute used to validate a user property.
/// </summary>
public class UserAttribute : PropertyPreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
        PropertyInfo propertyInfo, object obj, string value, IServiceProvider services)
    {
        _ = ulong.TryParse(value, out ulong userId);

        if (userId > long.MaxValue)
            return PreconditionResult.FromError("The specified number was too large!");

        if (context.Client.CurrentUser.Id == userId)
            return PreconditionResult.FromError("Whoa! You can't target me!");

        IUser? user = await context.Client.GetUserAsync(userId);

        if (user == null)
            return PreconditionResult.FromError($"User with ID `{userId}` doesn't exist.");

        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();

        if (db.Moderators.Any(admin => admin.Id == userId))
            return PreconditionResult.FromError($"Huh! {user.Mention} is an admin!");

        if (db.Scammers.Any(fraudster => fraudster.Id == userId))
            return PreconditionResult.FromError($"{user.Mention} has already been added!");

        return PreconditionResult.FromSuccess();
    }
}
