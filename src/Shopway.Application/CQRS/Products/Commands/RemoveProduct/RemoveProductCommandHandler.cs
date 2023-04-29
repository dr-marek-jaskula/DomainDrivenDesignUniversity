using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Application.Utilities;
using Shopway.Domain.Utilities;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandHandler : ICommandHandler<RemoveProductCommand, RemoveProductResponse>
{
    //Data to create dummy product that will be attached to database context
    private static readonly Result<ProductName> _validProductNameDummyResult = ProductName.Create("ToDelete");
    private static readonly Result<Price> _validPriceDummyResult = Price.Create(1m);
    private static readonly Result<UomCode> _validUomCodeDummyResult = UomCode.Create("kg");
    private static readonly Result<Revision> _validRevisionDummyResult = Revision.Create("0");

    private readonly IProductRepository _productRepository;

    public RemoveProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IResult<RemoveProductResponse>> Handle(RemoveProductCommand command, CancellationToken cancellationToken)
    {
        //Create a dummy product with proper product id to remove
        var productToRemove = Product.Create
        (
            id: command.Id,
            productName: _validProductNameDummyResult.Value,
            price: _validPriceDummyResult.Value,
            uomCode: _validUomCodeDummyResult.Value,
            revision: _validRevisionDummyResult.Value
        );

        //Attach the product to the ChangeTracker and set its status to Deleted
        _productRepository.Remove(productToRemove);

        return productToRemove
            .ToRemoveResponse()
            .ToResult()
            .ToTask();
    }
}