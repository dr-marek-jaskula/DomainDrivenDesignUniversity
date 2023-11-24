using RestSharp;
using Microsoft.EntityFrameworkCore;
using Shopway.Presentation.Controllers;
using Shopway.Tests.Integration.Persistence;
using Shopway.Tests.Integration.Abstractions;
using Shopway.Application.Features.Products.Commands.AddReview;
using Review = Shopway.Domain.Products.Review;
using static System.Threading.CancellationToken;
using static Shopway.Tests.Integration.Constants.Constants;
using static Shopway.Tests.Integration.Constants.Constants.CollectionName;
using static Shopway.Tests.Integration.Constants.Constants.IntegrationTest;
using Shopway.Domain.Products;

namespace Shopway.Tests.Integration.ControllersUnderTest.Reviews;

[Collection(ProductControllerCollection)]
[Trait(nameof(IntegrationTest), Api)]
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

    private async Task<Review?> GetReview(AddReviewCommand.AddReviewRequestBody body, ProductId productId)
    {
        return await _fixture
            .Context
            .Set<Review>()
            .Where(r => r.ProductId == productId)
            .Where(r => (decimal)(object)r.Stars == body.Stars)
            .Where(r => (string)(object)r.Title == body.Title)
            .Where(r => (string)(object)r.Description == body.Description)
            .FirstOrDefaultAsync(None);
    }
}