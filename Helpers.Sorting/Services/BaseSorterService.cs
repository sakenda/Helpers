
using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Services;

public abstract class BaseSorterService<TEntity, TKey> : IListSorterService<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected virtual bool AreEqual(TEntity entity1, TEntity entity2)
    {
        return entity1.Id.Equals(entity2.Id);
    }

    protected virtual bool IsNewer(TEntity entity1, TEntity entity2)
    {
        if (entity1.LastModified == null && entity2.LastModified == null)
            return false;
        if (entity1.LastModified == null)
            return false;
        if (entity2.LastModified == null)
            return true;

        return entity1.LastModified > entity2.LastModified;
    }

    public abstract SortResult<TEntity, TKey> Sort(IEnumerable<TEntity> sourceList, IEnumerable<TEntity> targetList);
}
