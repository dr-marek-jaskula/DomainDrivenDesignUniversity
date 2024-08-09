using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.MinimalEndpoints.Orders;

public sealed class OrderHeadersGroup : IEndpointGroup
{
    private const string Group = "OrderHeaders";
    private const string Tag = $"{Group}.MinimalEndpoints";

    public static IEndpointRouteBuilder RegisterEndpointGroup(IEndpointRouteBuilder app)
    {
        return app.MapGroup($"/minimal/{Group}")
            .WithTags(Tag);
    }
}
