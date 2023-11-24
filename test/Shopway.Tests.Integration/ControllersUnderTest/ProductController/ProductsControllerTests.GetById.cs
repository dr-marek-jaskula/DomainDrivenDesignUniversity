using RestSharp;
using Shopway.Domain.Errors;
using Shopway.Tests.Integration.Utilities;
using Shopway.Application.Features.Products.Queries;
using Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;
using Product = Shopway.Domain.Products.Product;
using static System.Net.HttpStatusCode;
using static Shopway.Tests.Integration.Constants.Constants;
using Shopway.Domain.Products;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    [Trait(nameof(IntegrationTest), IntegrationTest.Api)]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var expected = await _fixture.DataGenerator.AddProductAsync();

        var request = GetRequest(expected.Id.Value.ToString());
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var actual = response.Deserialize<ProductResponse>();
        actual.ShouldMatch(expected);
    }

    [Fact]
    [Trait(nameof(IntegrationTest), IntegrationTest.Api)]
    public async Task GetById_ShouldReturnErrorResponse_WhenProductNotExists()
    {
        //Arrange
        var invalidProductId = ProductId.New();

        var request = GetRequest(invalidProductId.Value.ToString());
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);

        //Act
        var response = await _restClient!.ExecuteGetAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ValidationProblemDetails>();
        problemDetails!.ShouldContain(Error.InvalidReference(invalidProductId.Value, nameof(Product)));
    }
}