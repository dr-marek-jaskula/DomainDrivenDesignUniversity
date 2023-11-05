using Shopway.Domain.Abstractions;
using Shopway.Infrastructure.FuzzySearch;

using Microsoft.Extensions.DependencyInjection;

public static class FuzzySearchRegistration
{
    internal static IServiceCollection RegisterFuzzySearch(this IServiceCollection services)
    {
        services.AddScoped<IFuzzySearchFactory, SymSpellFuzzySearchFactory>();

        return services;
    }
}