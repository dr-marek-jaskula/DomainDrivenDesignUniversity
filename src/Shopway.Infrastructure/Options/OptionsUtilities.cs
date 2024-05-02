using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

public static class OptionsUtilities
{
    public static TOptions GetOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, new()
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
    }

    public static TOptions GetOptions<TOptions>(this IApplicationBuilder app)
        where TOptions : class, new()
    {
        return app.ApplicationServices.GetRequiredService<IOptions<TOptions>>().Value;
    }
}
