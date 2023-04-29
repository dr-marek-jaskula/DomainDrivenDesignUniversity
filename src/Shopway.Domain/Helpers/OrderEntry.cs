using Shopway.Domain.Enums;

namespace Shopway.Domain.Helpers;

public sealed record OrderEntry
{
    public string PropertyName { get; init; } = string.Empty;
    public SortDirection SortDirection { get; init; }
    public int SortPriority { get; init; }
}