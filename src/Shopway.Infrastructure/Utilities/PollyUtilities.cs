using Polly;
using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;

namespace Shopway.Infrastructure.Utilities;

public static class PollyUtilities
{
    public static async Task<Result> ExecuteAndReturnResult(this ResiliencePipeline resiliencePipeline, Func<CancellationToken, ValueTask> callback, CancellationToken cancellationToken)
    {
		try
		{
            await resiliencePipeline.ExecuteAsync(callback, cancellationToken);
        }
		catch (Exception innerException)
		{
			return Result.Failure(Domain.Errors.Error.FromException(innerException));
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