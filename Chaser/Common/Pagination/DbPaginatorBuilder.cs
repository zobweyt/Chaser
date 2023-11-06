using Chaser.Data;
using Discord;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Chaser;

/// <summary>
/// Represents a builder class for making a <see cref="DbPaginator{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">The database entity type.</typeparam>
public sealed class DbPaginatorBuilder<TEntity> : BaseLazyPaginatorBuilder<DbPaginator<TEntity>, DbPaginatorBuilder<TEntity>>
    where TEntity : DbEntity
{
    private int _maxPerPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbPaginatorBuilder{TEntity}"/> class.
    /// </summary>
    public DbPaginatorBuilder()
    {
        WithPageFactory(GeneratePage);
        WithActionOnCancellation(ActionOnStop.DeleteMessage);
        WithActionOnTimeout(ActionOnStop.DisableInput);
        WithFooter(PaginatorFooter.None);
        AddOption(Emotes.Cross, PaginatorAction.Exit, ButtonStyle.Secondary);
        AddOption(Emotes.Navigation.Jump, PaginatorAction.Jump, ButtonStyle.Secondary);
        AddOption(Emotes.Navigation.Backward, PaginatorAction.Backward, ButtonStyle.Secondary);
        AddOption(Emotes.Navigation.Forward, PaginatorAction.Forward, ButtonStyle.Secondary);
    }

    /// <summary>
    /// Gets or sets the maximum number of items per page.
    /// </summary>
    public int MaxPerPage
    {
        get => _maxPerPage;
        set
        {
            if (value > EmbedBuilder.MaxFieldCount)
                throw new ArgumentException($"Value must be less than or equal to {EmbedBuilder.MaxFieldCount}.", nameof(MaxPerPage));
            _maxPerPage = value;
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="IQueryable{TEntity}"/> to be paginated.
    /// </summary>
    public IQueryable<TEntity> Set { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of emotes and the corresponding value generator functions.
    /// </summary>
    public Dictionary<Emote, Func<TEntity, object>> Pairs { get; set; } = new();

    public override DbPaginator<TEntity> Build()
    {
        WithMaxPageIndex((int)Math.Ceiling(Set.Count() / (double)MaxPerPage) - 1);
        return new(this);
    }

    /// <summary>
    /// Sets the maximum number of items per page.
    /// </summary>
    /// <param name="maxPerPage">The maximum number of items per page.</param>
    /// <returns>This builder.</returns>
    public DbPaginatorBuilder<TEntity> WithMaxPerPage(int maxPerPage)
    {
        MaxPerPage = maxPerPage;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="DbSet{TEntity}"/> to be paginated.
    /// </summary>
    /// <param name="set">The <see cref="DbSet{TEntity}"/>.</param>
    /// <returns>This builder.</returns>
    public DbPaginatorBuilder<TEntity> WithSet(IQueryable<TEntity> set)
    {
        Set = set;
        return this;
    }

    /// <summary>
    /// Adds a pair of emote and value generator function.
    /// </summary>
    /// <param name="emote">The emote key.</param>
    /// <param name="value">The value generator function.</param>
    /// <returns>This builder.</returns>
    public DbPaginatorBuilder<TEntity> AddPair(Emote emote, Func<TEntity, object> value)
    {
        Pairs.Add(emote, value);
        return this;
    }

    private PageBuilder GeneratePage(int pageIndex)
    {
        string entityTypeName = typeof(TEntity).Name.ToLower();
        string entitiesNumericQuantity = entityTypeName.ToQuantity(Set.Count());

        var pageBuilder = new PageBuilder()
            .WithTitle($"Browsing {entityTypeName.Pluralize()}")
            .WithFooter($"Page {pageIndex + 1}/{MaxPageIndex + 1}  •  {entitiesNumericQuantity}")
            .WithColor(Colors.Primary);

        foreach (TEntity entity in Set.OrderByDescending(x => x.IdentityAt).Skip(MaxPerPage * pageIndex).Take(MaxPerPage))
            GenerateField(pageBuilder, entity);

        return pageBuilder;
    }

    private void GenerateField(PageBuilder pageBuilder, TEntity entity)
    {
        var rows = Pairs.Select(p => $"{p.Key} {p.Value.Invoke(entity)}");

        var name = entity.Id.ToString();
        var value = string.Join(Environment.NewLine, rows);

        pageBuilder.AddField(name, value, true);
    }
}
