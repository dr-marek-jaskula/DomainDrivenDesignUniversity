using NBomber.CSharp;
using Shopway.Domain.EntityIds;
using Shopway.Tests.Performance.Scenarios;
using Shopway.Tests.Performance.Abstractions;
using static Shopway.Tests.Performance.Constants.Constants.PerformanceSkipReason;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests : ControllerTestsBase
{
    [Fact(Skip = SkipReason)]
    public void GetById_ShouldHandleAtLeast100RequestPerSecond()
    {
        var productId = ProductId.New();
        string url = $"{ShopwayApiUrl}/{ControllerUri}/{productId.Value}";

        var scenario = ProductScenarios.CreateGetScenario("GetProductByIdScenario", url, GetApiKey)
            .WithInit(async context => await InsertProduct(productId))
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(3)))
            .WithClean(async context => await DeleteProduct(productId));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
        
        DisplayStatistics(stats);
    }
}