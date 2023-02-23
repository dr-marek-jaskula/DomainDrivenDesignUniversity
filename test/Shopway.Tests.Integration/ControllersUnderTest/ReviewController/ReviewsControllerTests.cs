using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using Shopway.Presentation.Controllers;
using static Shopway.Tests.Integration.Constants.CollectionNames;
using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;

namespace Shopway.Tests.Integration.ControllersUnderTest.ReviewController;

[Collection(ProductControllerCollection)]
public sealed partial class ReviewsControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture;

    public ReviewsControllerTests(DatabaseFixture databaseFixture, DependencyInjectionContainerTestFixture containerTestFixture) 
        : base(containerTestFixture)
    {
        _fixture = databaseFixture;
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient($"{nameof(ProductsController)[..^Controller.Length]}/", _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }
}