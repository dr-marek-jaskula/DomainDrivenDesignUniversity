using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.MinimalEndpoints.Proxy;

public sealed class ProxyGroup : IEndpointGroup
{
    private const string Group = "Proxy";
    private const string Tag = $"{Group}.MinimalEndpoints";

    public static IEndpointRouteBuilder RegisterEndpointGroup(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/minimal/{Group}")
            .AllowAnonymous()
            .WithTags(Tag);
    }
}
