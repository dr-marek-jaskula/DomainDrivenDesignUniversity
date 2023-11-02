using Shopway.Domain.EntityKeys;
using Shopway.Application.Features.Products.Commands.BatchUpsertProduct;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Tests.Integration.Constants.Constants.ProductBatchUpsert;
using static Shopway.Application.Features.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

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
            return new BatchUpsertProductCommand(AsList(new ProductBatchUpsertRequest((ProductKey)ProductKey, (decimal)Price, UomCode)));
        }

        return new BatchUpsertProductCommand(AsList(Request));
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