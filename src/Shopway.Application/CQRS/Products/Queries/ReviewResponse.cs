using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Queries;

public sealed record ReviewResponse
(
    Guid Id,
    string Username,
    decimal Stars,
    string Title,
    string Description
)
    : IResponse;