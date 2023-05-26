using RestSharp;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Tests.Integration.Persistance;
using Shopway.Presentation.Controllers;
using static Shopway.Tests.Integration.Constants.CollectionNames;
using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;
using Shopway.Application.CQRS.Products.Commands.AddReview;
using Shopway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Threading.CancellationToken;

namespace Shopway.Tests.Integration.ControllersUnderTest.Reviews;

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

    private async Task<Review?> GetReview(AddReviewCommand.AddReviewRequestBody body)
    {
        return await _fixture
            .Context
            .Set<Review>()
            .Where(r => r.Stars.Value == body.Stars)
            .Where(r => r.Title.Value == body.Title)
            .Where(r => r.Description.Value == body.Description)
            .FirstOrDefaultAsync(None);
    }
}