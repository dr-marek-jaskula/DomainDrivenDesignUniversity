using RestSharp;
using Shopway.Domain.Entities;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using static Shopway.Tests.Integration.Collections.CollectionNames;

namespace Shopway.Tests.Integration.ControllersUnderTest.ReviewController;

[Collection(Product_Controller_Collection)]
public sealed partial class ReviewControllerTests : ControllerTestsBase, IAsyncLifetime
{
    private RestClient? _restClient;
    private readonly DatabaseFixture _fixture;

    public ReviewControllerTests(DatabaseFixture fixture) : base()
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _restClient = await RestClient($"{nameof(Product)}/", _fixture);
    }

    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }
}