using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Tests.Integration.Persistence;

/// <summary>
/// Test dependency injection container fixture
/// </summary>
public sealed class DependencyInjectionContainerTestFixture
{
    /// <summary>
    /// Test ServiceProvider
    /// </summary>
    public ServiceProvider ServiceProvider { get; set; }

    public DependencyInjectionContainerTestFixture()
    {
        ServiceProvider = ServiceProviderFactory.ServiceProvider;
    }
}
