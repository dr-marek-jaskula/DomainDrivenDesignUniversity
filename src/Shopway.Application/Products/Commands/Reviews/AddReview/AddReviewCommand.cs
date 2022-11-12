using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.AddReview;

public sealed record AddReviewCommand
(
    Guid ProductId,
    string Username,
    decimal Stars,
    string Title,
    string Description
) : ICommand<Guid>;
