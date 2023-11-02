using NBomber.CSharp;
using NBomber.Contracts;
using NBomber.Http.CSharp;
using static Shopway.Tests.Performance.Constants.Constants.Http;

namespace Shopway.Tests.Performance.Scenarios;

public static class ProductScenarios
{
    public static ScenarioProps CreateGetScenario(string scenarioName, string url, string apiKey)
    {
        return Scenario.Create(scenarioName, async context =>
        {
            var request = Http.CreateRequest(GET, url)
                    .WithHeader(ApiKey, apiKey);

            var response = await Http.Send(Client, request);

            return response;
        });
    }
}