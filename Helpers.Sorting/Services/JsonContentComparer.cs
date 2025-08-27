using Helpers.Sorting.Abstractions.Json;
using Helpers.Sorting.Common;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Helpers.Sorting.Services;

/// <summary>
/// Zentrale Klasse für JSON-basierten Inhaltsvergleich
/// </summary>
public class JsonContentComparer<TEntity, TKey>
    where TEntity : IJsonComparableSortableEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly JsonSerializerOptions _defaultOptions;

    public JsonContentComparer(JsonSerializerOptions defaultOptions = null!)
    {
        _defaultOptions = defaultOptions ?? CreateDefaultJsonOptions();
    }

    public bool AreEqual(TEntity entity1, TEntity entity2)
    {
        if (entity1 == null && entity2 == null) return true;
        if (entity1 == null || entity2 == null) return false;
        if (ReferenceEquals(entity1, entity2)) return true;

        try
        {
            // Verwende Entity-spezifische Optionen oder Fallback zu Default
            var options1 = entity1.GetJsonSerializerOptions() ?? _defaultOptions;
            var options2 = entity2.GetJsonSerializerOptions() ?? _defaultOptions;

            // Serialisiere beide Entitäten
            var json1 = SerializeWithIgnoredProperties(entity1, options1);
            var json2 = SerializeWithIgnoredProperties(entity2, options2);

            // Vergleiche JSON-Strings
            return string.Equals(json1, json2, StringComparison.Ordinal);
        }
        catch (Exception ex)
        {
            // Fallback zu Referenz-Vergleich wenn JSON-Serialisierung fehlschlägt
            System.Diagnostics.Debug.WriteLine($"JSON comparison failed: {ex.Message}");
            return ReferenceEquals(entity1, entity2);
        }
    }

    private string SerializeWithIgnoredProperties(TEntity entity, JsonSerializerOptions options)
    {
        var ignoredProperties = entity.GetIgnoredPropertiesForComparison();

        if (ignoredProperties?.Length > 0)
        {
            // Erstelle eine Kopie der Optionen mit ignorierten Properties
            var modifiedOptions = new JsonSerializerOptions(options);

            // Füge Contract Resolver hinzu der Properties ignoriert
            modifiedOptions.TypeInfoResolver = new IgnorePropertiesTypeInfoResolver(ignoredProperties);

            return JsonSerializer.Serialize(entity, modifiedOptions);
        }

        return JsonSerializer.Serialize(entity, options);
    }

    private static JsonSerializerOptions CreateDefaultJsonOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
    }
}
