using RestSharp;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(ProductControllerCollection)]
public sealed partial class ProductsControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture;

    public ProductsControllerTests(DatabaseFixture databaseFixture, DependencyInjectionContainerTestFixture containerTestFixture) 
        : base(containerTestFixture)
    {
        _fixture = databaseFixture;
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient(_controllerUri, _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

    /// <summary>
    /// Asserts product response.
    /// </summary>
    /// <param name="productResponse">Deserialized product response</param>
    private static void AssertProductResponse(ProductResponse productResponse, Product product, bool assertReviews = false)
    {
        productResponse.Id.Should().Be(product.Id.Value);
        productResponse.Price.Should().Be(product.Price.Value);
        productResponse.ProductName.Should().Be(product.ProductName.Value);
        productResponse.Revision.Should().Be(product.Revision.Value);
        productResponse.UomCode.Should().Be(product.UomCode.Value);

        if (assertReviews)
        {
            AssertReviewResponses(productResponse.Reviews, product.Reviews);
        }
    }

    /// <summary>
    /// Asserts review responses.
    /// </summary>
    /// <param name="reviewResponses">Deserialized review responses</param>
    /// <param name="reviews">Reviews to compare responses with</param>
    private static void AssertReviewResponses(IReadOnlyCollection<ReviewResponse> reviewResponses, IReadOnlyCollection<Review> reviews)
    {
        foreach (var reviewResponse in reviewResponses)
        {
            var review = reviews.First(review => review.Id.Value == reviewResponse.Id);
            AssertReviewResponse(reviewResponse, review);
        }
    }

    /// <summary>
    /// Asserts review response.
    /// </summary>
    /// <param name="reviewResponse">Deserialized review response</param>
    /// <param name="review">Review to compare response with</param>
    private static void AssertReviewResponse(ReviewResponse reviewResponse, Review review)
    {
        reviewResponse.Id.Should().Be(review.Id.Value);
        reviewResponse.Description.Should().Be(review.Description.Value);
        reviewResponse.Stars.Should().Be(review.Stars.Value);
        reviewResponse.Title.Should().Be(review.Title.Value);
        reviewResponse.Username.Should().Be(review.Username.Value);
    }
}