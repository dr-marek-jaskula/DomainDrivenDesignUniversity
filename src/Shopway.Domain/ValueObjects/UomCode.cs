using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Domain.ValueObjects;

public sealed class UomCode : ValueObject
{
    public readonly static string[] AllowedUomCodes = [ "pcs", "kg" ];

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