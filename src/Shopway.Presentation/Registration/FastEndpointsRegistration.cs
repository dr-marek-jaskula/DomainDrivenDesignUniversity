using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FastEndpointsRegistration
{
    internal static IServiceCollection RegisterEndpoints(this IServiceCollection services)
    {
        services
           .AddFastEndpoints();

        return services;
    }

    internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        app
            .UseFastEndpoints(options =>
            {
                options.Endpoints.RoutePrefix = "api/fast";

                options.Endpoints.Configurator = configure =>
                {
                    configure.Description(builder => builder.ProducesProblem(StatusCodes.Status400BadRequest));
                };
            })
            .UseSwaggerGen();

        return app;
    }
}
