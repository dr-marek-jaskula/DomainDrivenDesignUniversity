using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Products.DataProcessing.Mapping;

public sealed record ProductStaticMapping : IMapping<Product, DataTransferObject>
{
    public static readonly ProductStaticMapping Instance = new()
    {
        Id = true,
        ProductName = true,
        Revision = true,
        Price = true,
        UomCode = true,
    };

    public bool? Id { get; init; }
    public bool? ProductName { get; init; }
    public bool? Revision { get; init; }
    public bool? Price { get; init; }
    public bool? UomCode { get; init; }

    private bool IncludeId => Id is true;
    private bool IncludeProductName => ProductName is true;
    private bool IncludeRevision => Revision is true;
    private bool IncludePrice => Price is true;
    private bool IncludeUomCode => UomCode is true;

    public IQueryable<DataTransferObject> Apply(IQueryable<Product> queryable)
    {
        return queryable
            .Select(product => new DataTransferObject()
                .AddIf(IncludeId, nameof(product.Id), $"{product.Id}")
                .AddIf(IncludeUomCode, nameof(product.UomCode), $"{product.UomCode}")
                .AddIf(IncludePrice, nameof(product.Price), $"{product.Price}")
                .AddIf(IncludeRevision, nameof(product.Revision), $"{product.Revision}")
                .AddIf(IncludeProductName, nameof(product.ProductName), $"{product.ProductName}"));
    }
}