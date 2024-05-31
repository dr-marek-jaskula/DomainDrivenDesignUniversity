using Asp.Versioning;
using Asp.Versioning.Conventions;
using FastEndpoints.AspVersioning;
using Shopway.Presentation.Endpoints;

namespace Microsoft.Extensions.DependencyInjection;

internal static class VersioningRegistration
{
    private const string ApiVersionHeader = "api-version";

    internal static IServiceCollection RegisterVersioning(this IServiceCollection services)
    {
        services
            .SetControllersVersioning()
            .SetEndpointsVersioning();

        return services;
    }

    private static IServiceCollection SetControllersVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeader);
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection SetEndpointsVersioning(this IServiceCollection services)
    {
        services
           .AddVersioning(options =>
           {
               options.DefaultApiVersion = ApiVersion.Default;
               options.ReportApiVersions = true;
               options.AssumeDefaultVersionWhenUnspecified = true;
               options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeader);
           })
           .AddVersionSet(VersionGroup.Proxy, (1, 0), (2, 0))
           .AddVersionSet(VersionGroup.Products, (1, 0), (2, 0))
           .AddVersionSet(VersionGroup.Orders, (1, 0), (2, 0));

        return services;
    }

    private static IServiceCollection AddVersionSet(this IServiceCollection services, string versionSet, params (int major, int minor)[] versions)
    {
        VersionSets.CreateApi(versionSet, builder =>
        {
            foreach (var (major, minor) in versions)
            {
                builder.HasApiVersion(major, minor);
            }
        });

        return services;
    }
}
