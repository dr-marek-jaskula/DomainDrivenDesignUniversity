using Shopway.Application.Middlewares;

namespace Microsoft.Extensions.DependencyInjection;

public static class MiddlewaresRegistration
{
    public static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
    {
        //Order is not important
        services.AddScoped<ErrorHandlingMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        //Order is important
        app.UseMiddleware<ErrorHandlingMiddleware>();

        return app;
    }
}