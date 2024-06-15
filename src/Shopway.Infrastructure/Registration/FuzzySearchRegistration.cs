using Shopway.Application.Abstractions;
using Shopway.Infrastructure.FuzzySearch;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FuzzySearchRegistration
{
    internal static IServiceCollection RegisterFuzzySearch(this IServiceCollection services)
    {
        services.AddScoped<IFuzzySearchFactory, SymSpellFuzzySearchFactory>();

        return services;
    }
}
