using RestSharp;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.EntityKeys;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Tests.Integration.Constants.Constants;
using static Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities.CreateProductCommandUtility;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    [Trait(nameof(IntegrationTest), IntegrationTest.Api)]
    public async Task Create_ShouldReturnFailure_WhenProductExists()
    {
        //Arrange
        var product = await _fixture.DataGenerator.AddProductAsync();
        var productKey = ProductKey.From(product);

        var body = CreateProductCommand(productKey);

        var request = PostRequest(string.Empty, body);
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_CREATE);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ValidationProblemDetails>();
        problemDetails!.ShouldContain(Error.AlreadyExists(productKey));
    }
}