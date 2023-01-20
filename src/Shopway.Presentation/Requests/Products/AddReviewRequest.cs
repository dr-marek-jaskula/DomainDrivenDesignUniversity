namespace Shopway.Presentation.Requests.Products;

public sealed record AddReviewRequest
(
    decimal Stars,
    string Title,
    string Description
);