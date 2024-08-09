using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Shopway.Presentation.FastEndpoints.Products;

public sealed class ProductsGroup : Group
{
    private const string Group = "Products";
    private const string Tag = $"{Group}.FastEndpoints";

    public ProductsGroup()
    {
        Configure(Group, endpointDefinition =>
        {
            endpointDefinition
                .Description(builder => builder
                    .WithTags(Tag));
        });
    }
}
