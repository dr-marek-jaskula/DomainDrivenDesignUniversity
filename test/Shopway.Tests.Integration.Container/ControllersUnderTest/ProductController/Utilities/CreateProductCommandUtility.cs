using Shopway.Domain.EntityKeys;
using Shopway.Application.Features.Products.Commands.CreateProduct;
using static Shopway.Tests.Integration.Container.Constants.Constants.Product;

namespace Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities;

public static class CreateProductCommandUtility
{
    public static CreateProductCommand CreateProductCommand(ProductKey key, decimal? price = null, string? uomCode = null)
    {
        return new CreateProductCommand(key, price ?? Price, uomCode ?? UomCode);
    }
}