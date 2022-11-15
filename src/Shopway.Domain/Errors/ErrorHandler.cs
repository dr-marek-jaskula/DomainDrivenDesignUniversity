using Shopway.Domain.Results;

namespace Shopway.Domain.Errors;

public static class ErrorHandler
{
    /// <summary>
    /// Determine if any of given ValueObjects is in invalid state
    /// </summary>
    /// <param name="valueObjects"></param>
    /// <returns>First ValueObject validation Error</returns>
    public static Error FirstValueObjectErrorOrErrorNone(params Result[] valueObjects)
    {
        foreach (var valueObject in valueObjects)
        {
            if (valueObject.IsFailure)
            {
                return valueObject.Error;
            }
        }

        return Error.None;
    }
}

