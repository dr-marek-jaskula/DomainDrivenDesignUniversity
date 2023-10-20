using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries;

public sealed record ReviewResponse
(
    Ulid Id,
    string Username,
    decimal Stars,
    string Title,
    string Description
)
    : IResponse;