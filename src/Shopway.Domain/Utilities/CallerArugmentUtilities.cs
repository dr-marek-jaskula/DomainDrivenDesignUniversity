using System.Runtime.CompilerServices;

namespace Shopway.Domain.Utilities;

public static class CallerArugmentUtilities
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