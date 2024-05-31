using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Endpoints.Orders;

public sealed class OrdersGroup : Group
{
    private const string Prefix = "Orders";
    private const string Tag = $"{Prefix}.Endpoints";

    public OrdersGroup()
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
