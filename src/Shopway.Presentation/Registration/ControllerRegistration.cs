using ApiBehaviorOptions = Shopway.Presentation.Options.ApiBehaviorOptions;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ControllerRegistration
{
    internal static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly)
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = ApiBehaviorOptions.InvalidModelStateResponse);

        return services;
    }
}
