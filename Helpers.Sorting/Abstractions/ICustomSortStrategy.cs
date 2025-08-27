using Helpers.Sorting.Models;

namespace Helpers.Sorting.Abstractions;

public interface ICustomSortStrategy<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    IEnumerable<TEntity> Sort(IEnumerable<TEntity> entities, SortCriteria criteria);
}
