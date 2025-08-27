using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Models;

namespace Helpers.Sorting.Services;

/// <summary>
/// Service für Datenbank-CRUD-Operationen Sortierung
/// </summary>
public class DatabaseSorterService<TEntity, TKey> : BaseSorterService<TEntity, TKey>, IDatabaseSorterService<TEntity, TKey>
    where TEntity : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public override SortResult<TEntity, TKey> Sort(
        IEnumerable<TEntity> sourceList,
        IEnumerable<TEntity> targetList)
    {
        var dbResult = SortForDatabase(sourceList, targetList);
        return new SortResult<TEntity, TKey>
        {
            SortedEntities = dbResult.ToInsert
                .Concat(dbResult.ToUpdate)
                .ToList()
        };
    }

    public DatabaseSortResult<TEntity, TKey> SortForDatabase(
        IEnumerable<TEntity> databaseEntities,
        IEnumerable<TEntity> clientEntities)
    {
        var dbList = databaseEntities.ToList();
        var clientList = clientEntities.ToList();

        var result = new DatabaseSortResult<TEntity, TKey>();

        // Erstelle Lookups für bessere Performance
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
                // Neues Entity - Insert
                toInsert.Add(clientEntity);
            }
            else
            {
                var dbEntity = dbLookup[clientEntity.Id];

                // Prüfe ob Update notwendig ist
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

        // Sortierte Entities sind alle die verbleiben (Insert + Update + unveränderte)
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
        // Standardverhalten: Update wenn Client-Entity neuer ist
        return IsNewer(clientEntity, dbEntity);
    }
}
