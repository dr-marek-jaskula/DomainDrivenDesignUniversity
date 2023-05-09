using NBomber.CSharp;
using Shopway.Domain.EntityIds;
using Shopway.Tests.Performance.Abstractions;
using Shopway.Tests.Performance.Scenarios;
using static Shopway.Tests.Performance.Constants.OutputHelperContants;

namespace Shopway.Tests.Performance.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests : ControllerTestsBase
{
    [Fact]
    public void GetById_ShouldHandleAtleast100RequestPerSecond()
    {
        var productId = ProductId.New();
        string url = $"{ShopwayApiUrl}/{controllerUri}/{productId.Value}";

        var scenario = ProductScenarios.CreateGetScenario("GetProductByIdScenario", url, GetApiKey)
            .WithInit(async context => await InsertProduct(productId))
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(3)))
            .WithClean(async context => await DeleteProduct(productId));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        _outputHelper.WriteLine($"{OkCount}{stats.AllOkCount}");
        _outputHelper.WriteLine($"{FailCount}{stats.AllFailCount}");
        _outputHelper.WriteLine($"{AllCount}{stats.AllRequestCount}");
        _outputHelper.WriteLine($"{FailPercentage}{stats.AllFailCount / stats.AllRequestCount}");
    }
}