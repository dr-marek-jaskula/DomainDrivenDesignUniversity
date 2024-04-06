using Microsoft.Extensions.Options;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Shopway.Tests.Performance.Options;
using Shopway.Tests.Performance.Persistence;
using Shopway.Tests.Performance.Utilities;

namespace Shopway.Tests.Performance.Abstractions;

[CollectionDefinition(nameof(Constants.Constants.IntegrationTest.Performance))]
public sealed class PerformanceTestsBaseCollection
    : ICollectionFixture<DatabaseFixture>;

[Trait(nameof(Constants.Constants.IntegrationTest), Constants.Constants.IntegrationTest.Performance)]
[Collection(nameof(Constants.Constants.IntegrationTest.Performance))]
public abstract class PerformanceTestsBase(DatabaseFixture fixture, IHttpClientFactory httpClientFactory, IOptions<PerformanceTestOptions> options)
{
    protected readonly HttpClient _httpClient = httpClientFactory.CreateClient(nameof(PerformanceTestsBase));
    protected readonly DatabaseFixture _fixture = fixture;

    protected readonly LoadSimulation _loadSimulation = Simulation.Inject
    (
        options.Value.RequestsPerSecond,
        TimeSpan.FromSeconds(1),
        TimeSpan.FromMinutes(options.Value.DurationInMinutes)
    );

    protected readonly TimeSpan _warmUpDuration = TimeSpan.FromSeconds(options.Value.WarmUpDurationInSeconds);

    protected ScenarioProps CreateScenario(string scenarioName, string endpointUrl, HttpMethod httpMethod)
    {
        return Scenario.Create(scenarioName, async context =>
        {
            try
            {
                return await SendRequestWithoutBody(httpMethod, endpointUrl);
            }
            catch (Exception)
            {
                return Response.Fail();
            }
        });
    }

    protected ScenarioProps CreateScenario<TBody>(string scenarioName, string endpointUrl, HttpMethod httpMethod, Func<TBody> bodyFactory)
    {
        return Scenario.Create(scenarioName, async context =>
        {
            try
            {
                return await SendRequestWithBody(httpMethod, endpointUrl, bodyFactory());
            }
            catch (Exception)
            {
                return Response.Fail();
            }
        });
    }

    private async Task<IResponse> SendRequestWithBody<TBody>(HttpMethod httpMethod, string endpointUrl, TBody body)
    {
        var request = Http.CreateRequest(httpMethod.Method, endpointUrl)
            .WithBody(body.ToStringContent());

        return await Http.Send(_httpClient, request);
    }

    private async Task<IResponse> SendRequestWithoutBody(HttpMethod httpMethod, string endpointUrl)
    {
        var request = Http.CreateRequest(httpMethod.Method, endpointUrl);
        return await Http.Send(_httpClient, request);
    }
}