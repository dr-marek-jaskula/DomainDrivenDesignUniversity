using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;

namespace Shopway.Presentation.Abstractions;

public static class EndpointConventionBuilderUtilities
{
    public static IEndpointConventionBuilder WithVersion(this IEndpointConventionBuilder builder, string versionSet, int major, int minor)
    {
        return builder
            .WithVersionSet(versionSet)
            .MapToApiVersion(major, minor);
    }
}
