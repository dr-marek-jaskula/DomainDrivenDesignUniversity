using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed record class UomCode : ValueObject<string>
{
    public readonly static string[] AllowedUomCodes = ["pcs", "kg"];

    public static readonly Error Invalid = Error.New(
        $"{nameof(UomCode)}.{nameof(Invalid)}",
        $"{nameof(UomCode)} name must be: {AllowedUomCodes.Join(',')}.");

    private UomCode(string uomCode) : base(uomCode)
    {
    }

    public static ValidationResult<UomCode> Create(string uomCode)
    {
        var errors = Validate(uomCode);
        return errors.CreateValidationResult(() => new UomCode(uomCode));
    }

    public static IList<Error> Validate(string uomCode)
    {
        return EmptyList<Error>()
            .If(AllowedUomCodes.NotContains(uomCode), Invalid);
    }
}
