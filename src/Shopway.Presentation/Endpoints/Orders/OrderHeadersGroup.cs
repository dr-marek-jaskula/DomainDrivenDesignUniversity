using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Endpoints.Orders;

public sealed class OrderHeadersGroup : Group
{
    private const string Prefix = "OrderHeaders";
    private const string Tag = $"{Prefix}.Endpoints";

    public OrderHeadersGroup()
    {
        Configure(Prefix, endpointDefinition =>
        {
            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
