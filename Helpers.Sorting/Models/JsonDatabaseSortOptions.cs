using Helpers.Sorting.Abstractions.Json;
using Helpers.Sorting.Common;
using System.Text.Json;

namespace Helpers.Sorting.Models;

/// <summary>
/// Basis-Optionen für JSON-Sortierung (ohne Generics für einfache Verwendung)
/// </summary>
public class JsonDatabaseSortOptions : DatabaseSortOptions
{
    public UpdateDetectionStrategy UpdateDetectionStrategy { get; set; } = UpdateDetectionStrategy.JsonContentOnly;
    public JsonSerializerOptions GlobalJsonOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    public bool LogJsonComparisons { get; set; } = false;
}

/// <summary>
/// Erweiterte Optionen für JSON-basierte Datenbank-Sortierung
/// </summary>
public class JsonDatabaseSortOptions<TEntity, TKey> : DatabaseSortOptions<TEntity, TKey>
    where TEntity : IJsonComparableSortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public UpdateDetectionStrategy UpdateDetectionStrategy { get; set; } = UpdateDetectionStrategy.JsonContentOnly;
    public JsonSerializerOptions GlobalJsonOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false, 
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
    public bool LogJsonComparisons { get; set; } = false;
}

