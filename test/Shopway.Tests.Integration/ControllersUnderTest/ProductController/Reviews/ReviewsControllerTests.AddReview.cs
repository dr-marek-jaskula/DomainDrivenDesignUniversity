using RestSharp;
using Shopway.Domain.Entities;
using static System.Net.HttpStatusCode;
using static Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities.AddReviewCommandUtility;

namespace Shopway.Tests.Integration.ControllersUnderTest.Reviews;

public partial class ReviewsControllerTests
{
    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var product = await _fixture.DataGenerator.AddProduct();
        var body = CreateAddReviewCommand();

        var request = PostRequest($"{product.Id}/{Presentation.Controllers.ProductsController.Reviews}", body);

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