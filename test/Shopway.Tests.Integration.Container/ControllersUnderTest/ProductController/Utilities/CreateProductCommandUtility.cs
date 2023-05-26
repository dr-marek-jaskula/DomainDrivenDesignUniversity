﻿using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Domain.EntityKeys;
using static Shopway.Tests.Integration.Container.Constants.ProductConstants;

namespace Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities;

public static class CreateProductCommandUtility
{
    public static CreateProductCommand CreateProductCommand(ProductKey key, decimal? price = null, string? uomCode = null)
    {
        return new CreateProductCommand(key, price ?? Price, uomCode ?? UomCode);
    }
}