using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.Reviews.UpdateReview;

public sealed record UpdateReviewCommand
(
    Guid ProductId,
    Guid ReviewId,
    decimal Stars,
    string Description
) : ICommand<Guid>;
