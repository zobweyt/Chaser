using Chaser.Data;
using Fergun.Interactive.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Chaser;

/// <summary>
/// Represents a lazy paginator that provides enhanced pagination features for <see cref="DbSet{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">The database entity type.</typeparam>
public sealed class DbPaginator<TEntity> : BaseLazyPaginator
    where TEntity : DbEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbPaginator{TEntity}"/> class.
    /// </summary>
    /// <param name="builder">The builder used to configure the paginator.</param>
    public DbPaginator(DbPaginatorBuilder<TEntity> builder)
        : base(builder) { }
}
