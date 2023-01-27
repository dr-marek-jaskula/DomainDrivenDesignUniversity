using RestSharp;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Collections.CollectionNames;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(Product_Controller_Collection)]
public sealed class ReviewControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture;

    public ReviewControllerTests(DatabaseFixture fixture) : base()
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient("Product/", _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var generatedProductId = _fixture.DataGenerator.AddProductWithoutReviews();

        var body = new AddReviewCommand.AddReviewRequestBody(2, "CustomTitle", "Description");

        var request = PostRequest($"{generatedProductId.Result.Value}/review", body);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var addedReview = _fixture
            .Context
            .Set<Review>()
            .Where(r => r.Stars.Value == body.Stars)
            .Where(r => r.Title.Value == body.Title)
            .Where(r => r.Description.Value == body.Description)
            .FirstOrDefault();

        addedReview.Should().NotBeNull();
    }
}