using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Products.Errors;

public static partial class DomainErrors
{
    public static class UomCodeError
    {
        public static readonly Error Invalid = Error.New(
            $"{nameof(UomCode)}.{nameof(Invalid)}",
            $"{nameof(UomCode)} name must be: {UomCode.AllowedUomCodes.Join(',')}.");
    }
}