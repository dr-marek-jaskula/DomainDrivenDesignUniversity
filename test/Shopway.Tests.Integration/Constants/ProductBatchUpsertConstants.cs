using Shopway.Domain.EntityKeys;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Tests.Integration.Constants;

public sealed class ProductBatchUpsertConstants
{
    public static List<ProductBatchUpsertRequest> Requests = AsList
    (
        new ProductBatchUpsertRequest(ProductKey.Create("firstTestProduct", "1,0"), 100m, "pcs"),
        new ProductBatchUpsertRequest(ProductKey.Create("secondTestProduct", "2,0"), 50m, "kg"),
        new ProductBatchUpsertRequest(ProductKey.Create("thirdTestProduct", "3,0"), 10m, "pcs")
    );

    public static ProductBatchUpsertRequest Request = new ProductBatchUpsertRequest(ProductKey.Create("firstTestProduct", "1,0"), 100m, "pcs");
}