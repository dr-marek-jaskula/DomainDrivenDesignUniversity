using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Endpoints.Proxy;

public sealed class ProxyGroup : Group
{
    private const string Prefix = "Proxy";
    private const string Tag = $"{Prefix}.Endpoints";

    public ProxyGroup()
    {
        Configure(Prefix, endpointDefinition =>
        {
            endpointDefinition
                .AllowAnonymous();

            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
