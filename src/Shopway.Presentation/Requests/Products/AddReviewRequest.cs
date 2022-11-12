namespace Shopway.Presentation.Requests.Products;

public sealed record AddReviewRequest
(
    string Username,
    decimal Stars,
    string Title,
    string Description
);