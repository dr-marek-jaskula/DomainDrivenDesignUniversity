using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.EntityKeys;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Tests.Integration.Constants.Constants.ProductBatchUpsert;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;

public static class ProductBatchUpsertCommandUtility
{
    public static BatchUpsertProductCommand CreateProductBatchUpsertCommand(params ProductBatchUpsertRequest[] productBatchUpsertRequests)
    {
        if (productBatchUpsertRequests.IsNullOrEmpty())
        {
            return new BatchUpsertProductCommand(Requests);
        }

        return new BatchUpsertProductCommand(productBatchUpsertRequests);
    }

    public static BatchUpsertProductCommand CreateProductBatchUpsertCommandWithSingleRequest
    (
        ProductKey? ProductKey = null,
        decimal? Price = null,
        string? UomCode = null
    )
    {
        if (ProductKey is not null && Price is not null && UomCode is not null)
        {
            return new BatchUpsertProductCommand([new ProductBatchUpsertRequest((ProductKey)ProductKey, (decimal)Price, UomCode)]);
        }

        return new BatchUpsertProductCommand([Request]);
    }

    public static ProductBatchUpsertRequest CreateProductBatchUpsertRequest
    (
        ProductKey? ProductKey = null,
        decimal? Price = null,
        string? UomCode = null
    )
    {
        if (ProductKey is not null && Price is not null && UomCode is not null)
        {
            return new ProductBatchUpsertRequest((ProductKey)ProductKey, (decimal)Price, UomCode);
        }

        return Request;
    }
}