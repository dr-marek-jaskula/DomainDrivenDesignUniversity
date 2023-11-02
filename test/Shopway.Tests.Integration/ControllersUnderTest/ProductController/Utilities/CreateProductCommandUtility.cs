using Shopway.Application.Features.Products.Commands.CreateProduct;
using Shopway.Domain.EntityKeys;
using static Shopway.Tests.Integration.Constants.Constants.Product;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;

public static class CreateProductCommandUtility
{
    public static CreateProductCommand CreateProductCommand(ProductKey key, decimal? price = null, string? uomCode = null)
    {
        return new CreateProductCommand(key, price ?? Price, uomCode ?? UomCode);
    }
}