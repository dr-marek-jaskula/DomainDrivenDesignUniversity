using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Domain.Products.Errors;

public static partial class DomainErrors
{
    public static class StarsError
    {
        public static readonly Error Invalid = Error.New(
            $"{nameof(Stars)}.{nameof(Invalid)}",
            $"{nameof(Stars)} name must be: {Stars.AdmissibleStars.Join(',')}.");
    }
}