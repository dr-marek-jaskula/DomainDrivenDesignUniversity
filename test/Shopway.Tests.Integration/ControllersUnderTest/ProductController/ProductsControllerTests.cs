using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistence;
using static Shopway.Tests.Integration.Constants.Constants.CollectionName;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(ProductControllerCollection)]
public sealed partial class ProductsControllerTests(DatabaseFixture databaseFixture, DependencyInjectionContainerTestFixture containerTestFixture) 
    : ControllerTestsBase(containerTestFixture), IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture = databaseFixture;

    public async Task InitializeAsync()
    {
        _restClient = await RestClient(_controllerUri, _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }
}
