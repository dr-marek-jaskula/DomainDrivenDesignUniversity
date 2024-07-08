using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ConfigureSerialization
{
    internal static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(option =>
        {
            option.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            option.SerializerOptions.WriteIndented = true;
            option.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }
}
