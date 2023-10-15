using Shopway.Domain.Abstractions;
using Shopway.Infrastructure.FuzzySearch;

namespace Shopway.App.Registrations;

public static class FuzzySearchRegistration
{
    public static void RegisterFuzzySearch(this IServiceCollection services)
    {
        services.AddScoped<IFuzzySearchFactory, SymSpellFuzzySearchFactory>();
    }
}