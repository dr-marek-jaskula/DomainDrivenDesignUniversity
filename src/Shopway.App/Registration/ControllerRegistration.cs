namespace Shopway.App.Registration;

public static class ControllerRegistration
{
    public static void RegisterControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly);
    }
}
