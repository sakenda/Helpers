
namespace Helpers.Sorting.Common;

public static class EnumerableExtensions
{
    /// <summary>
    /// This Methods splits an IEnumerable into batches of a specified size.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
    {
        T[] bucket = null!;
        var count = 0;

        foreach (var item in source)
        {
            if (bucket == null)
                bucket = new T[size];

            bucket[count++] = item;

            if (count != size)
                continue;

            yield return bucket.Select(x => x);

            bucket = null!;
            count = 0;
        }

        if (bucket != null && count > 0)
            yield return bucket.Take(count);
    }
}
