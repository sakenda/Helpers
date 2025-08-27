
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Abstractions;

public interface IListSorterService<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    SortResult<TEntity, TKey> Sort(IEnumerable<TEntity> sourceList, IEnumerable<TEntity> targetList);
}
