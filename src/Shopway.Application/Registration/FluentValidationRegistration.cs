using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FluentValidationRegistration
{
    internal static IServiceCollection RegisterFluentValidation(this IServiceCollection services)
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
