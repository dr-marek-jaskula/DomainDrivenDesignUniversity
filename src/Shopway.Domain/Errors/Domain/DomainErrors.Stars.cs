using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class StarsError
    {
        public static readonly Error Invalid = new(
            $"{nameof(Stars)}.{nameof(Invalid)}",
            $"{nameof(Stars)} name must be: {string.Join(',', Stars.AdmissibleStars)}");
    }
}