using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Xunit.Abstractions;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public sealed partial class ProductsControllerTests
{
    private readonly ITestOutputHelper _outputHelper;
    private const string controllerUri = "products";
    private const string GetApiKey = "d3f72374-ef67-42cb-b25b-fbfee58b1054";

    public ProductsControllerTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    private async Task InsertProduct(ProductId productId)
    {
        await fixture.DataGenerator.AddProduct(productId);
    }

    private async Task DeleteProduct(ProductId productId)
    {
        var entity = await fixture.Context
            .Set<Product>()
            .Where(product => product.Id == productId)
            .FirstAsync();

        fixture.Context
            .Set<Product>()
            .Remove(entity);

        await fixture.Context.SaveChangesAsync();
    }
}