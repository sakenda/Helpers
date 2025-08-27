using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Abstractions.Json;
using Helpers.Sorting.Common;
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Services;

public class JsonComparableDatabaseSorterService<TEntity, TKey> : JsonComparableBaseSorterService<TEntity, TKey>, IDatabaseSorterService<TEntity, TKey>
    where TEntity : IJsonComparableSortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly JsonDatabaseSortOptions _options;

    public JsonComparableDatabaseSorterService(JsonDatabaseSortOptions options = null!, JsonContentComparer<TEntity, TKey> contentComparer = null!)
        : base(contentComparer)
    {
        _options = options ?? new JsonDatabaseSortOptions();
    }

    public override SortResult<TEntity, TKey> Sort(IEnumerable<TEntity> sourceList, IEnumerable<TEntity> targetList)
    {
        var dbResult = SortForDatabase(sourceList, targetList);
        return new SortResult<TEntity, TKey>
        {
            SortedEntities = dbResult.ToInsert.Concat(dbResult.ToUpdate).ToList()
        };
    }

    public DatabaseSortResult<TEntity, TKey> SortForDatabase(IEnumerable<TEntity> databaseEntities, IEnumerable<TEntity> clientEntities)
    {
        var dbList = databaseEntities.ToList();
        var clientList = clientEntities.ToList();

        var result = new DatabaseSortResult<TEntity, TKey>();

        var dbLookup = dbList.ToDictionary(e => e.Id, e => e);
        var clientLookup = clientList.ToDictionary(e => e.Id, e => e);

        var toInsert = new List<TEntity>();
        var toUpdate = new List<TEntity>();
        var toDelete = new List<TKey>();

        // Finde Inserts und Updates
        foreach (var clientEntity in clientList)
        {
            if (!dbLookup.ContainsKey(clientEntity.Id))
            {
                toInsert.Add(clientEntity);
            }
            else
            {
                var dbEntity = dbLookup[clientEntity.Id];

                if (RequiresUpdate(dbEntity, clientEntity))
                {
                    toUpdate.Add(clientEntity);
                }
            }
        }

        // Finde Deletes
        foreach (var dbEntity in dbList)
        {
            if (!clientLookup.ContainsKey(dbEntity.Id))
            {
                toDelete.Add(dbEntity.Id);
            }
        }

        result.ToInsert = toInsert.AsReadOnly();
        result.ToUpdate = toUpdate.AsReadOnly();
        result.ToDelete = toDelete.AsReadOnly();

        // Sortierte Entities
        var unchangedEntities = dbList.Where(db =>
            clientLookup.ContainsKey(db.Id) &&
            !RequiresUpdate(db, clientLookup[db.Id])).ToList();

        result.SortedEntities = toInsert
            .Concat(toUpdate)
            .Concat(unchangedEntities)
            .OrderBy(e => e.Id)
            .ToList()
            .AsReadOnly();

        return result;
    }

    protected virtual bool RequiresUpdate(TEntity dbEntity, TEntity clientEntity)
    {
        switch (_options.UpdateDetectionStrategy)
        {
            case UpdateDetectionStrategy.LastModifiedOnly:
                return IsNewer(clientEntity, dbEntity);

            case UpdateDetectionStrategy.JsonContentOnly:
                return !AreContentEqual(dbEntity, clientEntity);

            case UpdateDetectionStrategy.JsonContentAndLastModified:
                return !AreContentEqual(dbEntity, clientEntity) && IsNewer(clientEntity, dbEntity);

            case UpdateDetectionStrategy.JsonContentOrLastModified:
                return !AreContentEqual(dbEntity, clientEntity) || IsNewer(clientEntity, dbEntity);

            default:
                return !AreContentEqual(dbEntity, clientEntity);
        }
    }
}
