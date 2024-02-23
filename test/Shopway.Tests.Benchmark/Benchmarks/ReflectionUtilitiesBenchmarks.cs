using BenchmarkDotNet.Attributes;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Tests.Performance.Benchmarks;

public class ReflectionUtilitiesBenchmarks
{
    private Product _product;
    private Func<Product, object> _getter;
    private Func<Product, ProductName> _getterProductName;

    [GlobalSetup]
    public void Setup()
    {
        var productId = ProductId.New();
        var productName = ProductName.Create("BenchmarkName").Value;
        var price = Price.Create(5).Value;
        var uomCode = UomCode.Create(UomCode.AllowedUomCodes.First()).Value;
        var revision = Revision.Create("2.0").Value;

        _product = Product.Create
        (
            productId,
            productName,
            price,
            uomCode,
            revision
        );

        _getter = (Func<Product, object>)Delegate
            .CreateDelegate(typeof(Func<Product, object>), null, typeof(Product).GetProperty(nameof(ProductName))!.GetGetMethod()!);

        _getterProductName = (Func<Product, ProductName>)Delegate
            .CreateDelegate(typeof(Func<Product, ProductName>), null, typeof(Product).GetProperty(nameof(ProductName))!.GetGetMethod()!);
    }

    [Benchmark(Baseline = true)]
    public string StandardGetPropertyAsString()
    {
        return _product.ProductName.ToString();
    }

    [Benchmark]
    public string ReflectionGetPropertyAsString()
    {
        return typeof(Product).GetProperty(nameof(ProductName))!.GetValue(_product)!.ToString()!;
    }

    [Benchmark]
    public string GetPropertyAsString_DelegateCached()
    {
        return _getter(_product).ToString()!;
    }

    [Benchmark]
    public string GetPropertyAsString_DelegateCachedWithExactType()
    {
        return _getterProductName(_product).ToString()!;
    }

    [Benchmark]
    public string GetPropertyAsString_DelegateNotCached()
    {
        return _product.GetPropertyAsString(nameof(ProductName));
    }
}