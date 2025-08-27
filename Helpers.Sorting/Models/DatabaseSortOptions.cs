
using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Common;

namespace Helpers.Sorting.Models;

/// <summary>
/// Basis-Konfigurationsoptionen für Database Sorter
/// </summary>
public class DatabaseSortOptions
{
    public UpdateStrategy UpdateStrategy { get; set; } = UpdateStrategy.LastModifiedWins;
    public bool IgnoreDeletes { get; set; } = false;
    public bool IgnoreInserts { get; set; } = false;
    public bool IgnoreUpdates { get; set; } = false;
}

/// <summary>
/// Generische Konfigurationsoptionen für Database Sorter mit Custom Comparer
/// </summary>
/// <typeparam name="TEntity">Entitätstyp</typeparam>
/// <typeparam name="TKey">Schlüsseltyp</typeparam>
public class DatabaseSortOptions<TEntity, TKey> : DatabaseSortOptions
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Benutzerdefinierte Vergleichsfunktion für Updates
    /// </summary>
    public Func<TEntity, TEntity, bool> CustomUpdateComparer { get; set; } = default!;
}
