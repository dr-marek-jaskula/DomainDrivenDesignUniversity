using RestSharp;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Tests.Integration.Collections.CollectionNames;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(Product_Controller_Collection)]
public sealed class ProductControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture;

    public ProductControllerTests(DatabaseFixture fixture) : base()
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient(_controllerUri, _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

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