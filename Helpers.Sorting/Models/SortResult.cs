using Helpers.Sorting.Abstractions;

namespace Helpers.Sorting.Models;

public class SortResult<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public IReadOnlyList<TEntity> SortedEntities { get; set; } = new List<TEntity>();
    public int TotalCount => SortedEntities.Count;
    public DateTime SortedAt { get; set; } = DateTime.UtcNow;

}
