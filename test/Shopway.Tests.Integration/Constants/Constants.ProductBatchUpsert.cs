using Shopway.Domain.EntityKeys;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;
using static Shopway.Tests.Integration.Constants.Constants.Product;

namespace Shopway.Tests.Integration.Constants;

public static partial class Constants
{
    public sealed class ProductBatchUpsert
    {
        public static List<ProductBatchUpsertRequest> Requests =
        [
            new ProductBatchUpsertRequest(ProductKey.Create(Name.FirstTestProduct, Revision.One), Price.Expensive, UomCode.Pcs),
            new ProductBatchUpsertRequest(ProductKey.Create(Name.SecondTestProduct, Revision.Two), Price.Balanced, UomCode.Kg),
            new ProductBatchUpsertRequest(ProductKey.Create(Name.ThirdTestProduct, Revision.Three), Price.Cheap, UomCode.Pcs)
        ];

        public static ProductBatchUpsertRequest Request = new(ProductKey.Create(Name.FirstTestProduct, Revision.One), Price.Expensive, UomCode.Kg);
    }
}
