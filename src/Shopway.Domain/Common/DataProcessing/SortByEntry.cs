using Shopway.Domain.Common.Enums;

namespace Shopway.Domain.Common.DataProcessing;

public sealed record SortByEntry
{
    public required string PropertyName { get; init; }
    public required SortDirection SortDirection { get; init; }
    public required int SortPriority { get; init; }
}