using Discord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaser.Data;

/// <summary>
/// Represents a base implementation of common properties for database entities.
/// </summary>
public abstract class DbEntity : IEntity<ulong>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public virtual ulong Id { get; init; }

    /// <summary>
    /// Gets or sets the identity date and time for this object.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual DateTimeOffset IdentityAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the computed date and time for this object.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public virtual DateTimeOffset ComputedAt { get; set; } = DateTimeOffset.UtcNow;
}
