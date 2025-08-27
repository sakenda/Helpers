using Helpers.Sorting.Models;

namespace Helpers.Sorting.Abstractions;

public interface IDatabaseSorterService<TEntity, TKey> : IListSorterService<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    DatabaseSortResult<TEntity, TKey> SortForDatabase(IEnumerable<TEntity> databaseEntities, IEnumerable<TEntity> clientEntities);
}
