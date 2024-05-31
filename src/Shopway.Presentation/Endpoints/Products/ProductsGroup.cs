using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.Endpoints.Products;

public sealed class ProductsGroup : Group
{
    private const string Prefix = "Products";
    private const string Tag = $"{Prefix}.Endpoints";

    public ProductsGroup()
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
