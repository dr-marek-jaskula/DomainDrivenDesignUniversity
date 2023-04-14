using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Microsoft.Extensions.DependencyInjection;

public static class VersioningRegistration
{
    private const string ApiVersionHeader = "api-version";

    public static IServiceCollection RegisterVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeader);
        });

        return services;
    }
}