using Chaser.Data;
using Discord;
using Discord.Interactions;

namespace Chaser;

public class EntityTypeConverter<TEntity> : TypeConverter<TEntity>
    where TEntity : DbEntity
{
    public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.User;

    public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
    {
        if (option.Value is not IEntity<ulong> value)
            throw new InvalidOperationException($"Unexpected value: {option.Value}");

        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();

        TEntity? entity = await db.Set<TEntity>().FindAsync(value.Id);

        if (entity != null)
            return TypeConverterResult.FromSuccess(entity);

        return TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{typeof(TEntity).Name} with ID `{value.Id}` doesn't exist.");
    }
}
