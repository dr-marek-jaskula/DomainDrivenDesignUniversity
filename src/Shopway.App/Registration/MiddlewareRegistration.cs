using Shopway.App.Middlewares;

namespace Shopway.App.Registration;

public static class MiddlewaresRegistration
{
    public static void RegisterMiddlewares(this IServiceCollection services)
    {
        //Order is not important
        services.AddScoped<ErrorHandlingMiddleware>();
    }

    public static void UseMiddlewares(this WebApplication app)
    {
        //Order is important
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}