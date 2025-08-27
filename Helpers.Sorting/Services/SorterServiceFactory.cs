using Helpers.Sorting.Abstractions.Json;
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Services;

public static partial class SorterServiceFactory
{
    public static JsonComparableDatabaseSorterService<TEntity, TKey> CreateJsonComparableDatabaseSorter<TEntity, TKey>(
        JsonDatabaseSortOptions options = null!,
        JsonContentComparer<TEntity, TKey> contentComparer = null!)
        where TEntity : IJsonComparableSortableEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return new JsonComparableDatabaseSorterService<TEntity, TKey>(options, contentComparer);
    }

}