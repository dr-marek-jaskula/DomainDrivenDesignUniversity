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

        var simulations = Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(3));

        var scenario = CreateScenario(GetProductById, url, HttpMethod.Get)
            .WithInit(async context => await InsertProduct(productId))
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(simulations)
            .WithClean(async context => await DeleteProduct(productId));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReport(GetProductById, nameof(GetById_ShouldHandleAtLeast100RequestPerSecond))
            .Run();
    }
}