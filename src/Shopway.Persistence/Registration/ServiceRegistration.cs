using Microsoft.Extensions.DependencyInjection;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Persistence.Specifications;

namespace Shopway.Persistence.Registration;

public static class ServiceRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Services
        services.AddSingleton(typeof(ILikeProvider<>), typeof(LikeProvider<>));

        return services;
    }
}