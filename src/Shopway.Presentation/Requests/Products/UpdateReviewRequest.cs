using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Requests.Products;

public sealed record UpdateReviewRequest
(
    decimal? Stars,
    string? Description
) : IRequest;