using RestSharp;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var generatedProductId = await _fixture.DataGenerator.AddProductWithoutReviews();

        var request = GetRequest(generatedProductId.Value.ToString());

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var deserializedResponse = response.DeserializeResponseResult<ProductResponse>();
        deserializedResponse.Id.Should().Be(generatedProductId.Value);
    }
}