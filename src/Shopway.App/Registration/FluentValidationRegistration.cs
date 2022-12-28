using FluentValidation;
using FluentValidation.AspNetCore;

namespace Shopway.App.Registration;

public static class FluentValidationRegistration
{
    public static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation(options => //Fluent Validation (Models -> Validators)
            {
                options.DisableDataAnnotationsValidation = true;

                ValidatorOptions.Global.LanguageManager.Enabled = false;
            })
            .AddValidatorsFromAssembly(
                Shopway.Application.AssemblyReference.Assembly,
                includeInternalTypes: true);

        return services;
    }
}
