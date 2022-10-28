using Microsoft.Extensions.Options;
using Shopway.App.Options;

namespace Shopway.App.Registration;

public static class OptionsRegistration
{
    public static void RegisterOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();
    }
}
