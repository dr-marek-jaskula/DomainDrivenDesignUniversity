using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.Products.Commands.AddReview;

public sealed record AddReviewCommand
(
    ProductId ProductId,
    string Username,
    decimal Stars,
    string Title,
    string Description
) : ICommand<Guid>;
