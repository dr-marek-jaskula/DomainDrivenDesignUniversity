using RestSharp;
using Shopway.Domain.Products;
using static Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities.AddReviewCommandUtility;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.Reviews;

public partial class ReviewsControllerTests
{
    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var product = await _fixture.DataGenerator.AddProductAsync();
        var body = CreateAddReviewCommand();

        var request = PostRequest($"{product.Id}/{Presentation.Controllers.ProductsController.Reviews}", body);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        Review? addedReview = await GetReview(body, product.Id);
        addedReview.Should().NotBeNull();
    }
}
