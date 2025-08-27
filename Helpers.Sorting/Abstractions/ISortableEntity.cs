
namespace Helpers.Sorting.Abstractions;

public interface ISortableEntity<TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; }
    DateTime? LastModified { get; }

}
