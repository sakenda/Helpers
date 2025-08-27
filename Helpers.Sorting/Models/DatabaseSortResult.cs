using Helpers.Sorting.Abstractions;

namespace Helpers.Sorting.Models;

public class DatabaseSortResult<TEntity, TKey> : SortResult<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public IReadOnlyList<TEntity> ToInsert { get; set; } = new List<TEntity>();
    public IReadOnlyList<TEntity> ToUpdate { get; set; } = new List<TEntity>();
    public IReadOnlyList<TKey> ToDelete { get; set; } = new List<TKey>();

    public int InsertCount => ToInsert.Count;
    public int UpdateCount => ToUpdate.Count;
    public int DeleteCount => ToDelete.Count;

    public bool HasChanges => InsertCount > 0 || UpdateCount > 0 || DeleteCount > 0;
}

