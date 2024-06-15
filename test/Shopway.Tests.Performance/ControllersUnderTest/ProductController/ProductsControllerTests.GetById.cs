using NBomber.CSharp;
using Shopway.Domain.Products;
using Shopway.Tests.Performance.Utilities;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    private const string GetApiKey = "d3f72374-ef67-42cb-b25b-fbfee58b1054";
    private const string GetProductById = nameof(GetProductById);

    [SkipForLocalEnvFact]
    public void GetById_ShouldHandleAtLeast100RequestPerSecond()
    {
        var productId = ProductId.New();
        string url = $"{ControllerUri}/{productId.Value}";

        _httpClient.WithApiKey(GetApiKey);

        var scenario = CreateScenario(GetProductById, url, HttpMethod.Get)
            .WithInit(async context => await InsertProduct(productId))
            .WithSetup(_loadSimulation, _warmUpDuration)
            .WithClean(async context => await DeleteProduct(productId));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReport(GetProductById, nameof(GetById_ShouldHandleAtLeast100RequestPerSecond))
            .Run();
    }
}
