using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

public sealed record RemoveReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId
) : ICommand<RemoveReviewResponse>;