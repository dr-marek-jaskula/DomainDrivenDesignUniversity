using Shopway.Application.Abstractions;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Utilities;

public static class ResponseUtilities
{
    public static IResult<TResponse> ToResult<TResponse>(this TResponse response)
        where TResponse : class, IResponse
    {
        return Result.Create(response);
    }

    public static IResult<TResponse> ToResult<TResponse>(this ValidationResult<TResponse> response)
        where TResponse : class, IResponse
    {
        return response;
    }
}