using Shopway.Domain.Common.Results;

namespace Shopway.Domain.Common.Utilities;

public static class ResultUtilities
{
    public static Task<TResult> ToTask<TResult>(this TResult result)
        where TResult : class, IResult
    {
        return Task.FromResult(result);
    }

    public static TValue? OrDefault<TResult, TValue>(this TResult result, TValue? defaultValue = default)
        where TResult : class, IResult<TValue>
    {
        return result.IsSuccess
            ? result.Value
            : defaultValue;
    }

    public static bool IsResult(this Type type)
    {
        return type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IResult));
    }

    public static bool IsGenericResult(this Type type)
    {
        return type.IsGenericType && type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IResult));
    }

    public static Type? GetUnderlyingType(this Type type)
    {
        return type.IsGenericResult()
            ? type.GetGenericArguments()[0]
            : null;
    }
}