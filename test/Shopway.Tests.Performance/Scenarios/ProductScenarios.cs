using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using static Shopway.Tests.Performance.Constants.Constants.Http;

namespace Shopway.Tests.Performance.Scenarios;

public static class ProductScenarios
{
    public static ScenarioProps CreateGetScenario(string scenarioName, string url, string apiKey)
    {
        using HttpClient client = new();

        return Scenario.Create(scenarioName, async context =>
        {
            var request = Http.CreateRequest(GET, url)
                    .WithHeader(ApiKey, apiKey);

            return await Http.Send(client, request);
        });
    }
}