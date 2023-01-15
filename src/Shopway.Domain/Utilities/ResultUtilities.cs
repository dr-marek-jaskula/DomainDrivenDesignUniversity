using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Utilities;

public static class ResultUtilities
{
    public static Task<TResult> ToTask<TResult>(this TResult result)
        where TResult : class, IResult
    {
        return Task.FromResult(result);
    }
}