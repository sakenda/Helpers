using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Common;
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Services;

/// <summary>
/// Erweiterte Version des DatabaseSorterService mit zusätzlichen Optionen
/// </summary>
public class AdvancedDatabaseSorterService<TEntity, TKey> : DatabaseSorterService<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly DatabaseSortOptions<TEntity, TKey> _options;

    public AdvancedDatabaseSorterService(DatabaseSortOptions<TEntity, TKey> options = null!)
    {
        _options = options ?? new DatabaseSortOptions<TEntity, TKey>();
    }

    // Überladung für Kompatibilität mit nicht-generischen Optionen
    public AdvancedDatabaseSorterService(DatabaseSortOptions baseOptions)
    {
        _options = new DatabaseSortOptions<TEntity, TKey>
        {
            UpdateStrategy = baseOptions.UpdateStrategy,
            IgnoreDeletes = baseOptions.IgnoreDeletes,
            IgnoreInserts = baseOptions.IgnoreInserts,
            IgnoreUpdates = baseOptions.IgnoreUpdates
        };
    }

    protected override bool RequiresUpdate(TEntity dbEntity, TEntity clientEntity)
    {
        switch (_options.UpdateStrategy)
        {
            case UpdateStrategy.AlwaysUpdate:
                return true;

            case UpdateStrategy.NeverUpdate:
                return false;

            case UpdateStrategy.LastModifiedWins:
                return IsNewer(clientEntity, dbEntity);

            case UpdateStrategy.CustomComparison:
                return _options.CustomUpdateComparer?.Invoke(dbEntity, clientEntity) ?? false;

            default:
                return base.RequiresUpdate(dbEntity, clientEntity);
        }
    }

    public DatabaseSortResult<TEntity, TKey> SortForDatabaseWithBatching(
        IEnumerable<TEntity> databaseEntities,
        IEnumerable<TEntity> clientEntities,
        int batchSize = 1000)
    {
        var allResults = new List<DatabaseSortResult<TEntity, TKey>>();
        var clientBatches = clientEntities.Batch(batchSize);

        foreach (var batch in clientBatches)
        {
            var batchResult = SortForDatabase(databaseEntities, batch);
            allResults.Add(batchResult);
        }

        // Kombiniere alle Batch-Ergebnisse
        return CombineResults(allResults);
    }

    private DatabaseSortResult<TEntity, TKey> CombineResults(
        List<DatabaseSortResult<TEntity, TKey>> results)
    {
        return new DatabaseSortResult<TEntity, TKey>
        {
            ToInsert = results.SelectMany(r => r.ToInsert).ToList().AsReadOnly(),
            ToUpdate = results.SelectMany(r => r.ToUpdate).ToList().AsReadOnly(),
            ToDelete = results.SelectMany(r => r.ToDelete).ToList().AsReadOnly(),
            SortedEntities = results.SelectMany(r => r.SortedEntities).ToList().AsReadOnly()
        };
    }
}
