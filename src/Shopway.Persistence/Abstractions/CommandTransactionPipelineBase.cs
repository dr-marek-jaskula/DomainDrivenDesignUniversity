using MediatR;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Abstractions;

public class CommandTransactionPipelineBase<TCommandResponse>
    where TCommandResponse : IResult
{
    protected readonly IUnitOfWork<ShopwayDbContext> UnitOfWork;

    public CommandTransactionPipelineBase(IUnitOfWork<ShopwayDbContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

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