using Shopway.App.Middlewares;

namespace Shopway.App.Registration;

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