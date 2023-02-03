using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Tests.Integration.Configurations;
using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;

namespace Shopway.Tests.Integration.Persistance;

public static class ServiceProviderFactory
{
    public static ServiceProvider ServiceProvider { get; }

    static ServiceProviderFactory()
    {
        ServiceProvider = BuildServiceProvider();
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();

        var configurations = AppsettingsConfiguration.GetConfiguration();

        serviceCollection
            .RegisterDatabaseContext(true)
            .AddTransient(_ => configurations)
            .AddTransient(_ => configurations.GetTestOptions<IntegrationTestsUrlOptions>(IntegrationTestsUrl));

        return serviceCollection.BuildServiceProvider();
    }

    private static TOptions GetTestOptions<TOptions>(this IConfiguration configuration, string sectionName) 
        where TOptions : new()
    {
        var configurationValue = new TOptions();
        configuration.GetSection(sectionName).Bind(configurationValue);
        return configurationValue;
    }
}