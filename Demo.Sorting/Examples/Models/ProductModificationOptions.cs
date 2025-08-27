
namespace Demo.Sorting.Examples.Models;

/// <summary>
/// Konfigurationsoptionen für die Produktmodifikation
/// </summary>
public class ProductModificationOptions
{
    /// <summary>
    /// Anzahl der zu aktualisierenden Produkte
    /// </summary>
    public int UpdateCount { get; set; } = 10;

    /// <summary>
    /// Anzahl der neu hinzuzufügenden Produkte
    /// </summary>
    public int InsertCount { get; set; } = 5;

    /// <summary>
    /// Anzahl der zu löschenden Produkte
    /// </summary>
    public int DeleteCount { get; set; } = 3;

    /// <summary>
    /// Ob auch Updates mit älterer LastModified Zeit erstellt werden sollen
    /// </summary>
    public bool IncludeOlderUpdates { get; set; } = false;

    /// <summary>
    /// Ob die Reihenfolge der Ergebnisse gemischt werden soll
    /// </summary>
    public bool ShuffleOrder { get; set; } = false;
}
