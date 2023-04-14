using Shopway.Application.Middlewares;

namespace Microsoft.Extensions.DependencyInjection;

public static class MiddlewaresRegistration
{
    public static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
    {
        //Order is not important
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<RequestTimeMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
    {
        //Order is important
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestTimeMiddleware>();

        return app;
    }
}