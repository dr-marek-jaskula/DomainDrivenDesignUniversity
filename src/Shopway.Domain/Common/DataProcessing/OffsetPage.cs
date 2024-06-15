using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Common.DataProcessing;

public sealed record OffsetPage : IOffsetPage
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}
