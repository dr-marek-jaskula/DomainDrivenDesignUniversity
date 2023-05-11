using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Shopway.Tests.Performance.Constants;

namespace Shopway.Tests.Performance.Scenarios;

public static class ProductScenarios
{
    public static ScenarioProps CreateGetScenario(string scenarioName, string url, string apiKey)
    {
        return Scenario.Create(scenarioName, async context =>
        {
            var request = Http.CreateRequest(HttpConstants.GET, url)
                    .WithHeader(HttpConstants.ApiKey, apiKey);

            var response = await Http.Send(HttpConstants.HttpClient, request);

            return response;
        });
    }
}