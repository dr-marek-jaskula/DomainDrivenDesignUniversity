using Shopway.Domain.Results;

namespace Shopway.Domain.Errors;

public static class ErrorHandler
{
    /// <summary>
    /// Used to determine if any of given ValueObjects is in Invalid state
    /// </summary>
    /// <param name="valueObjects"></param>
    /// <returns>First ValueObject validation Error</returns>
    public static Error FindFirstValueObjectError(params Result[] valueObjects)
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

