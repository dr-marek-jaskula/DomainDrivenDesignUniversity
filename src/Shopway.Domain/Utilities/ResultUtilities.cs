using Microsoft.VisualBasic;
using Shopway.Domain.Abstractions;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace Shopway.Domain.Utilities;

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
}