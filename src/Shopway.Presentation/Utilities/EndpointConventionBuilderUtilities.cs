using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Utilities;

public static class EndpointConventionBuilderUtilities
{
    public static IEndpointConventionBuilder WithVersion(this IEndpointConventionBuilder builder, string versionSet, int major, int minor)
    {
        return builder
            .WithVersionSet(versionSet)
            .MapToApiVersion(major, minor);
    }

    public static IEndpointConventionBuilder WithSummaryAuth(this RouteHandlerBuilder builder, string summary)
    {
        return builder
            .WithSummary($"{summary} (Auth)");
    }

    public static IEndpointConventionBuilder WithSummaryApiKey(this RouteHandlerBuilder builder, string summary)
    {
        return builder
            .WithSummary($"{summary} (ApiKey)");
    }
}
