using Helpers.Sorting.Common;

namespace Helpers.Sorting.Models;

public class SortCriteria
{
    public string PropertyName { get; set; } = string.Empty;
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
    public List<SortCriteria> ThenBy { get; set; } = new List<SortCriteria>();
}

