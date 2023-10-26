using FluentValidation;
using FluentValidation.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class FluentValidationRegistration
{
    public static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly
            (
                Shopway.Application.AssemblyReference.Assembly,
                includeInternalTypes: true
            );

        return services;
    }
}
