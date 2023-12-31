using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class UomCode : ValueObject
{
    public readonly static string[] AllowedUomCodes = ["pcs", "kg"];

    public new string Value { get; }

    private UomCode(string value)
    {
        Value = value;
    }

    //For EF Core
    private UomCode()
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
            .If(AllowedUomCodes.NotContains(uomCode), UomCodeError.Invalid);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}