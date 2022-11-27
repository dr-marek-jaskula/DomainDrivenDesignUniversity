using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.Products.Commands.Reviews.UpdateReview;

public sealed record UpdateReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId,
    decimal? Stars,
    string? Description
) : ICommand<Guid>;
