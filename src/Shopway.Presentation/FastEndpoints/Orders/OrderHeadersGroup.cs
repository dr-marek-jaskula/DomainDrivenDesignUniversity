using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.FastEndpoints.Orders;

public sealed class OrderHeadersGroup : Group
{
    private const string Group = "OrderHeaders";
    private const string Tag = $"{Group}.FastEndpoints";

    public OrderHeadersGroup()
    {
        Configure(Group, endpointDefinition =>
        {
            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
