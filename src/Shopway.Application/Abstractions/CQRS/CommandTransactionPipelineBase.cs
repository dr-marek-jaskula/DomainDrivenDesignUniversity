using MediatR;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;

namespace Shopway.Application.Abstractions.CQRS;

public class CommandTransactionPipelineBase<TCommandResponse>
    where TCommandResponse : IResult
{
    protected readonly IUnitOfWork<ShopwayDbContext> UniteOfWork;

    public CommandTransactionPipelineBase(IUnitOfWork<ShopwayDbContext> uniteOfWork)
    {
        UniteOfWork = uniteOfWork;
    }

    protected async Task<TCommandResponse> BeginTransactionAsync(RequestHandlerDelegate<TCommandResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await UniteOfWork.BeginTransactionAsync(cancellationToken);
        var result = await next();

        if (result.IsSuccess)
        {
            await UniteOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        else
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        return result;
    }
}