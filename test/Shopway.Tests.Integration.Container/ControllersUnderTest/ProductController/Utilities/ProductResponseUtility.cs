using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.Products;

namespace Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities;

public static class ProductResponseUtility
{
    /// <summary>
    /// Asserts product response.
    /// </summary>
    /// <param name="productResponse">Deserialized product response</param>
    /// <param name="product">Product to compare response with</param>
    /// <param name="assertReviews">Flag that indicates whether to assert reviews</param>
    public static void ShouldMatch(this ProductResponse productResponse, Product product, bool assertReviews = false)
    {
        productResponse.Id.Should().Be(product.Id.Value);
        productResponse.Price.Should().Be(product.Price.Value);
        productResponse.ProductName.Should().Be(product.ProductName.Value);
        productResponse.Revision.Should().Be(product.Revision.Value);
        productResponse.UomCode.Should().Be(product.UomCode.Value);

        if (assertReviews)
        {
            productResponse.Reviews.ShouldMatch(product.Reviews);
        }
    }

    /// <summary>
    /// Asserts review responses.
    /// </summary>
    /// <param name="reviewResponses">Deserialized review responses</param>
    /// <param name="reviews">Reviews to compare responses with</param>
    public static void ShouldMatch(this IReadOnlyCollection<ReviewResponse> reviewResponses, IReadOnlyCollection<Review> reviews)
    {
        foreach (var reviewResponse in reviewResponses)
        {
            var review = reviews.First(review => review.Id.Value == reviewResponse.Id);
            reviewResponse.ShouldMatch(review);
        }
    }

    /// <summary>
    /// Asserts review response.
    /// </summary>
    /// <param name="reviewResponse">Deserialized review response</param>
    /// <param name="review">Review to compare response with</param>
    public static void ShouldMatch(this ReviewResponse reviewResponse, Review review)
    {
        reviewResponse.Id.Should().Be(review.Id.Value);
        reviewResponse.Description.Should().Be(review.Description.Value);
        reviewResponse.Stars.Should().Be(review.Stars.Value);
        reviewResponse.Title.Should().Be(review.Title.Value);
        reviewResponse.Username.Should().Be(review.Username.Value);
    }
}
