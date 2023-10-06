using Shopway.Domain.Abstractions.Common;

namespace Shopway.Domain.Common;

public sealed record OffsetPage : IOffsetPage
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}