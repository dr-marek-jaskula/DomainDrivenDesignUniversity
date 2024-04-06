using Polly;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;

namespace Shopway.Infrastructure.Policies;

public static class PollyPipelineUtilities
{
    public static async Task<Result> ExecuteAndReturnResult(this ResiliencePipeline resiliencePipeline, Func<CancellationToken, ValueTask> callback, CancellationToken cancellationToken)
    {
        try
        {
            await resiliencePipeline.ExecuteAsync(callback, cancellationToken);
        }
        catch (Exception innerException)
        {
            return Result.Failure(Error.FromException(innerException));
        }

        return Result.Success();
    }

    public static Result ExecuteAndReturnResult(this ResiliencePipeline resiliencePipeline, Action callback)
    {
        try
        {
            resiliencePipeline.Execute(callback);
        }
        catch (Exception exception)
        {
            return Result.Failure(Error.FromException(exception));
        }

        return Result.Success();
    }
}