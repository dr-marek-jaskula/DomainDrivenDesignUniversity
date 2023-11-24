using MediatR;
using Shopway.Domain.Common.Results;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Abstractions;

public class CommandTransactionPipelineBase<TCommandResponse>(IUnitOfWork<ShopwayDbContext> unitOfWork)
    where TCommandResponse : IResult
{
    protected readonly IUnitOfWork<ShopwayDbContext> UnitOfWork = unitOfWork;

    protected async Task<TCommandResponse> BeginTransactionAsync(RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await UnitOfWork.BeginTransactionAsync(cancellationToken);

        var result = await next();

        if (result.IsSuccess)
        {
            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        return result;
    }
}