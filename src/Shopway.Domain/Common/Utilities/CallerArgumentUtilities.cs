using System.Runtime.CompilerServices;

namespace Shopway.Domain.Common.Utilities;

public static class CallerArgumentUtilities
{
    public static (T? Parameter, string CallerParameterName) WithName<T>
    (
        this T? parameter,
        [CallerArgumentExpression(nameof(parameter))] string callerParameterName = ""
    )
    {
        return (parameter, callerParameterName);
    }
}