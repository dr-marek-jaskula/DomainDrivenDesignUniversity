using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Utilities.ResultUtilities;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

internal sealed class RemoveProductCommandHandler : ICommandHandler<RemoveProductCommand, RemoveProductResponse>
{
    private readonly IProductRepository _productRepository;

    public RemoveProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<IResult<RemoveProductResponse>> Handle(RemoveProductCommand command, CancellationToken cancellationToken)
    {
        _productRepository.Remove(command.Id);

        return command.Id
            .ToRemoveResponse()
            .ToResult()
            .ToTask();
    }
}