using Chaser.Data;
using Discord;
using Discord.Interactions;

namespace Chaser;

public class EntityTypeReader<TEntity> : TypeReader<TEntity>
    where TEntity : DbEntity
{
    public override async Task<TypeConverterResult> ReadAsync(IInteractionContext context, string option, IServiceProvider services)
    {
        _ = ulong.TryParse(option, out ulong entityId);
        
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ChaserDbContext>();
        
        TEntity? entity = await db.Set<TEntity>().FindAsync(entityId);

        if (entity != null)
            return TypeConverterResult.FromSuccess(entity);

        return TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"{typeof(TEntity).Name} with ID `{entityId}` doesn't exist.");
    }
}
