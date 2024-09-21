using RestSharp;
using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Products;
using Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using Product = Shopway.Domain.Products.Product;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    [IntegrationTest.Api]
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
    [IntegrationTest.Api]
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
