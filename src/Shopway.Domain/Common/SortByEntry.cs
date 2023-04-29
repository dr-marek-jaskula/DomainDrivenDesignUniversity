using Shopway.Domain.Enums;

namespace Shopway.Domain.Common;

public sealed record SortByEntry
{
    public string PropertyName { get; init; } = string.Empty;
    public SortDirection SortDirection { get; init; }
    public int SortPriority { get; init; }
}