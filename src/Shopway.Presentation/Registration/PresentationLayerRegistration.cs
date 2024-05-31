using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Scrutor;

namespace Microsoft.Extensions.DependencyInjection;

public static class PresentationLayerRegistration
{
    public static IServiceCollection RegisterPresentationLayer(this IServiceCollection services)
    {
        services
            .RegisterControllers()
            .RegisterOpenApi()
            .RegisterVersioning()
            .RegisterSecurity()
            .RegisterEndpoints();

        services.Scan(selector => selector
            .FromAssemblies(
                Shopway.Presentation.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    public static IApplicationBuilder UsePresentationLayer(this IApplicationBuilder app, IHostEnvironment environment)
    {
        app
            .ConfigureOpenApi(environment.IsDevelopment())
            .UseAuthorization()
            .UseCorsPolicy()
            .UseEndpoints();

        return app;
    }
}
