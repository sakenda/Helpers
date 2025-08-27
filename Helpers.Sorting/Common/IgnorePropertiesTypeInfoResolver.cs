using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Helpers.Sorting.Common;

/// <summary>
/// Custom TypeInfoResolver für das Ignorieren spezifischer Properties
/// </summary>
public class IgnorePropertiesTypeInfoResolver : IJsonTypeInfoResolver
{
    private readonly HashSet<string> _ignoredProperties;

    public IgnorePropertiesTypeInfoResolver(string[] ignoredProperties)
    {
        _ignoredProperties = new HashSet<string>(ignoredProperties, StringComparer.OrdinalIgnoreCase);
    }

    public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var typeInfo = JsonTypeInfo.CreateJsonTypeInfo(type, options);

        if (typeInfo.Kind == JsonTypeInfoKind.Object)
        {
            foreach (var property in typeInfo.Properties)
            {
                if (_ignoredProperties.Contains(property.Name))
                {
                    property.ShouldSerialize = (_, _) => false;
                }
            }
        }

        return typeInfo;
    }
}

