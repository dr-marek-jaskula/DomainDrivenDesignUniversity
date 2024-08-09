using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.FastEndpoints.Proxy;

public sealed class ProxyGroup : Group
{
    private const string Group = "Proxy";
    private const string Tag = $"{Group}.FastEndpoints";

    public ProxyGroup()
    {
        Configure(Group, endpointDefinition =>
        {
            endpointDefinition
                .AllowAnonymous();

            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
