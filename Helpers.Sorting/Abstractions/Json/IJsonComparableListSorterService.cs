namespace Helpers.Sorting.Abstractions.Json;

/// <summary>
/// Interface für Services mit JSON-basiertem Inhaltsvergleich
/// </summary>
public interface IJsonComparableListSorterService<TEntity, TKey> : IListSorterService<TEntity, TKey>
    where TEntity : IJsonComparableSortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Vergleicht zwei Entitäten basierend auf JSON-Inhalt
    /// </summary>
    bool AreContentEqual(TEntity entity1, TEntity entity2);
}
