using Shopway.Domain.Products;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Domain.Common.Utilities.ResultUtilities;

namespace Shopway.Application.Features.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandHandler(IProductRepository productRepository) 
    : ICommandHandler<RemoveProductCommand, RemoveProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public Task<IResult<RemoveProductResponse>> Handle(RemoveProductCommand command, CancellationToken cancellationToken)
    {
        _productRepository.Remove(command.Id);

        return command.Id
            .ToRemoveResponse()
            .ToResult()
            .ToTask();
    }
}