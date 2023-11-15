using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using static Shopway.Domain.Utilities.ResultUtilities;

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