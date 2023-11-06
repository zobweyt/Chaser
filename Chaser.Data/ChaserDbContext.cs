using Microsoft.EntityFrameworkCore;

namespace Chaser.Data;

/// <summary>
/// An implementation of the <see cref="DbContext"/> for this solution.
/// </summary>
public class ChaserDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChaserDbContext"/>.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public ChaserDbContext(DbContextOptions<ChaserDbContext> options)
        : base(options) { }

    /// <summary>
    /// Gets or sets the moderators entities database set.
    /// </summary>
    public DbSet<Moderator> Moderators { get; set; }

    /// <summary>
    /// Gets or sets the scammers entities database set.
    /// </summary>
    public DbSet<Scammer> Scammers { get; set; }
}
