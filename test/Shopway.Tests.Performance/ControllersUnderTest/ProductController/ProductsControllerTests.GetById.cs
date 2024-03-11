using NBomber.Contracts.Stats;
using NBomber.CSharp;
using Shopway.Domain.Products;
using Shopway.Tests.Performance.Abstractions;
using Shopway.Tests.Performance.Scenarios;
using Shopway.Tests.Performance.Utilities;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests : ControllerTestsBase
{
    private const string GetProductByIdScenario = nameof(GetProductByIdScenario);

    [SkipForLocalEnvFact]
    public void GetById_ShouldHandleAtLeast100RequestPerSecond()
    {
        var productId = ProductId.New();
        string url = $"{ShopwayApiUrl}/{ControllerUri}/{productId.Value}";

        var scenario = ProductScenarios.CreateGetScenario(GetProductByIdScenario, url, GetApiKey)
            .WithInit(async context => await InsertProduct(productId))
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(3)))
            .WithClean(async context => await DeleteProduct(productId));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithTestSuite(nameof(ProductsControllerTests))
            .WithTestName(nameof(GetById_ShouldHandleAtLeast100RequestPerSecond))
            .WithTargetScenarios(GetProductByIdScenario)
            .WithReportFileName($"Reprot_{GetProductByIdScenario}")
            .WithReportFolder(ReportsDirectory)
            .WithReportFormats(ReportFormat.Html)
            .WithReportingInterval(TimeSpan.FromSeconds(10))
            .Run();

        DisplayStatistics(stats);
    }
}