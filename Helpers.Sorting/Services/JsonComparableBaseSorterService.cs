using Helpers.Sorting.Abstractions.Json;

namespace Helpers.Sorting.Services;

/// <summary>
/// Erweiterte Basis-Klasse mit JSON-Vergleichsfunktionalität
/// </summary>
public abstract class JsonComparableBaseSorterService<TEntity, TKey> : BaseSorterService<TEntity, TKey>, IJsonComparableListSorterService<TEntity, TKey>
    where TEntity : IJsonComparableSortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly JsonContentComparer<TEntity, TKey> _contentComparer;

    protected JsonComparableBaseSorterService(JsonContentComparer<TEntity, TKey> contentComparer = null!)
    {
        _contentComparer = contentComparer ?? new JsonContentComparer<TEntity, TKey>();
    }

    public virtual bool AreContentEqual(TEntity entity1, TEntity entity2)
    {
        return _contentComparer.AreEqual(entity1, entity2);
    }
}
