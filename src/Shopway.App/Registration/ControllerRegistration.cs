namespace Shopway.App.Registration;

public static class ControllerRegistration
{
    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly);

        return services;
    }
}
