using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Entities;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.Products.Commands.RemoveReview;

public sealed record RemoveReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId
) : ICommand<Guid>;