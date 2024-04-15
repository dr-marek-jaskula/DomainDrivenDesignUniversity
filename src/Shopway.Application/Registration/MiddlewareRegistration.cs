using Microsoft.AspNetCore.Builder;
using Shopway.Application.Middlewares;

namespace Microsoft.Extensions.DependencyInjection;

internal static class MiddlewaresRegistration
{
    internal static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
    {
        //Order is not important
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<RequestTimeMiddleware>();

        return services;
    }

    internal static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        //Order is important
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestTimeMiddleware>();

        return app;
    }
}