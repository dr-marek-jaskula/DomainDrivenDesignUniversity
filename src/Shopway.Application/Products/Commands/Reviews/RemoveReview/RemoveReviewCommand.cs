using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.RemoveReview;

public sealed record RemoveReviewCommand
(
    Guid ProductId,
    Guid ReviewId
) : ICommand<Guid>;