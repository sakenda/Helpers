using System.Text.Json;

namespace Helpers.Sorting.Abstractions.Json;

/// <summary>
/// Erweiterte Interface für Entitäten mit JSON-basierter Inhaltsvergleich
/// </summary>
/// <typeparam name="TKey">Typ des Primärschlüssels</typeparam>
public interface IJsonComparableSortableEntity<TKey> : ISortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Optionale Konfiguration für JSON-Serialisierung
    /// Wenn null, werden Standard-Optionen verwendet
    /// </summary>
    JsonSerializerOptions? GetJsonSerializerOptions() => null;

    /// <summary>
    /// Eigenschaften die beim Vergleich ignoriert werden sollen
    /// z.B. LastModified, CreatedAt, etc.
    /// </summary>
    string[] GetIgnoredPropertiesForComparison() => new[] { nameof(ISortableEntity<TKey>.LastModified) };

}
