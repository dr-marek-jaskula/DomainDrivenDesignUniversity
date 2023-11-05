using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsUtilities
{
    public static TOptions GetOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, new()
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
    }
}
