using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using static Shopway.Tests.Integration.Constants.CollectionNames;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(ProductControllerCollection)]
public sealed partial class ProductsControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;

    public ProductsControllerTests(ShopwayApiFactory apiFactory)
        : base(apiFactory)
    {
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient(httpClient, fixture);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}