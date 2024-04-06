using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Tests.Performance.Client;
using Shopway.Tests.Performance.Options;
using Shopway.Tests.Performance.Persistence;

namespace Shopway.Tests.Performance;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("performancesettings.json")
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton<DatabaseFixture>()
            .RegisterTestClient(configuration);

        services.Configure<PerformanceTestOptions>(configuration.GetSection(nameof(PerformanceTestOptions)));
    }
}