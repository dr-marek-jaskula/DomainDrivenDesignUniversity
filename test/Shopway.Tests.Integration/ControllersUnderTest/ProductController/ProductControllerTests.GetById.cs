using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using static Shopway.Tests.Integration.Collections.CollectionNames;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(Product_Controller_Collection)]
public sealed class ProductControllerTests : ControllerTestsBase
{
    private readonly RestClient _restClient;

    public ProductControllerTests() : base()
    {
        _restClient = RestClient(_controllerUri);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var request = GetRequest($"/e0d00df6-6724-4561-b332-0d9bd900898b");

        //Act
        var response = await _restClient.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);
    }
}