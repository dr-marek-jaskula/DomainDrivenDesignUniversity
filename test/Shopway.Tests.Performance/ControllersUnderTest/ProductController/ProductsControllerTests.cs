using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shopway.Domain.Products;
using Shopway.Tests.Performance.Abstractions;
using Shopway.Tests.Performance.Options;
using Shopway.Tests.Performance.Persistence;
using static System.Threading.CancellationToken;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public sealed partial class ProductsControllerTests(DatabaseFixture databaseFixture, IHttpClientFactory httpClientFactory, IOptions<PerformanceTestOptions> options) 
    : PerformanceTestsBase(databaseFixture, httpClientFactory, options)
{
    private const string ControllerUri = "products";

    private async Task InsertProduct(ProductId productId)
    {
        await _fixture.DataGenerator.AddProduct(productId);
    }

    private async Task DeleteProduct(ProductId productId)
    {
        var entity = await _fixture.Context
            .Set<Product>()
            .Where(product => product.Id == productId)
            .FirstAsync(None);

        _fixture.Context
            .Set<Product>()
            .Remove(entity);

        await _fixture.Context.SaveChangesAsync();
    }
}