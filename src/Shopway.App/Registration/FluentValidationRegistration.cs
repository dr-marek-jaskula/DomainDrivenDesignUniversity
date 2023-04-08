using FluentValidation;
using FluentValidation.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class FluentValidationRegistration
{
    public static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
                ValidatorOptions.Global.LanguageManager.Enabled = false;
            })
            .AddValidatorsFromAssembly
            (
                Shopway.Application.AssemblyReference.Assembly,
                includeInternalTypes: true
            );

        return services;
    }
}
