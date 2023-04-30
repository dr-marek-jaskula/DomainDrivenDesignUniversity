using Shopway.Domain.Enums;

namespace Shopway.Domain.Common;

public sealed record SortByEntry
{
    public required string PropertyName { get; init; }
    public required SortDirection SortDirection { get; init; }
    public required int SortPriority { get; init; }
}