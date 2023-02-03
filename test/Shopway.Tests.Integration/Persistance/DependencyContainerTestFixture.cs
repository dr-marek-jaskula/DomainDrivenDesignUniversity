using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Tests.Integration.Persistance;

public sealed class DependencyInjectionContainerTestFixture
{
    public ServiceProvider ServiceProvider { get; set; }

	public DependencyInjectionContainerTestFixture()
	{
		ServiceProvider = ServiceProviderFactory.ServiceProvider;
	}
}