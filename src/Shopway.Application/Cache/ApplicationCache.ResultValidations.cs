using Shopway.Application.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Results.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;

namespace Shopway.Application.Cache;

public static partial class ApplicationCache
{
    private const string Errors = "errors";
    public static readonly FrozenDictionary<Type, Func<Error[], IValidationResult>> ValidationResultCache;

    private static FrozenDictionary<Type, Func<Error[], IValidationResult>> CreateValidationResultCache()
    {
        Dictionary<Type, Func<Error[], IValidationResult>> resultValidationsCache = [];

        var responseTypes = Application.AssemblyReference.Assembly
            .GetTypesWithAnyMatchingInterface(i => i.Name.Contains(nameof(IResponse)))
            .Where(type => type.IsInterface is false);

        foreach (var type in responseTypes)
        {
            if (type.IsGenericType)
            {
                continue;
            }

            var validationMethod = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(type)
                .GetMethod(nameof(ValidationResult.WithErrors))!;

            var validationDelegate = CompileFunc(validationMethod, type);

            var resultType = typeof(IResult<>).MakeGenericType(type);
            resultValidationsCache.TryAdd(resultType, validationDelegate);
        }

        //Add for Result and IResult
        resultValidationsCache.TryAdd(typeof(Result), ValidationResult.WithErrors);
        resultValidationsCache.TryAdd(typeof(IResult), ValidationResult.WithErrors);

        return resultValidationsCache.ToFrozenDictionary();
    }

    private static Func<Error[], IValidationResult> CompileFunc(MethodInfo methodInfo, Type type)
    {
        var parameter = Expression.Parameter(typeof(Error[]), Errors);
        var call = Expression.Call(null, methodInfo, parameter);

        var lambda = Expression.Lambda<Func<Error[], IValidationResult>>
        (
            Expression.Convert(call, typeof(IValidationResult)),
            false,
            parameter
        );

        return lambda.Compile();
    }
}
