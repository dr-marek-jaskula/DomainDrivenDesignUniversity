using Shopway.Domain.Abstractions.Common;

namespace Shopway.Domain.Common;

public sealed record Page : IPage
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}