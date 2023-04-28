using Shopway.Domain.Abstractions;

namespace Shopway.Application.CQRS;

public sealed record Page : IPage
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}