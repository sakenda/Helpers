
namespace Helpers.Sorting.Common;

public enum SortDirection
{
    Ascending,
    Descending
}

public enum DatabaseOperation
{
    Insert,
    Update,
    Delete
}

public enum UpdateStrategy
{
    LastModifiedWins,
    AlwaysUpdate,
    NeverUpdate,
    CustomComparison
}

public enum UpdateDetectionStrategy
{
    /// <summary>
    /// Nur LastModified-Zeitstempel vergleichen (Standard-Verhalten)
    /// </summary>
    LastModifiedOnly,

    /// <summary>
    /// Nur JSON-Inhalt vergleichen (empfohlen für genaue Erkennung)
    /// </summary>
    JsonContentOnly,

    /// <summary>
    /// JSON-Inhalt unterschiedlich UND Client ist neuer
    /// </summary>
    JsonContentAndLastModified,

    /// <summary>
    /// JSON-Inhalt unterschiedlich ODER Client ist neuer
    /// </summary>
    JsonContentOrLastModified
}
