using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class UomCodeError
    {
        public static readonly Error Invalid = Error.New(
            $"{nameof(UomCode)}.{nameof(Invalid)}",
            $"{nameof(UomCode)} name must be: {string.Join(',', UomCode.AllowedUomCodes)}.");
    }
}