using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using static Shopway.Tests.Integration.Container.Constants.Constants;
using static Shopway.Tests.Integration.Container.Constants.Constants.CollectionName;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

[Collection(WebApplicationFactoryCollection)]
[Trait(nameof(IntegrationTest), IntegrationTest.Api)]
public sealed partial class ProductsControllerTests : ControllerTestsBase
{
    private readonly RestClient _restClient;

    public ProductsControllerTests(ShopwayApiFactory apiFactory)
        : base(apiFactory)
    {
        var clientUri = new Uri($"{shopwayApiUrl}{controllerUrl}");
        _restClient = RestClient(httpClient, new RestClientOptions(clientUri));
    }
}
