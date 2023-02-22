using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class UomCode : ValueObject
{
    public readonly static string[] AllowedUomCodes = new string[2] { "pcs", "kg" };

    public string Value { get; }

    private UomCode(string value)
    {
        Value = value;
    }

    //For EF Core
    private UomCode()
    {
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static ValidationResult<UomCode> Create(string uomCode)
    {
        var errors = Validate(uomCode);
        return errors.CreateValidationResult(() => new UomCode(uomCode));
    }

    public static List<Error> Validate(string uomCode)
    {
        var errors = Empty<Error>();

        if (!AllowedUomCodes.Contains(uomCode))
        {
            errors.Add(UomCodeError.Invalid);
        }

        return errors;
    }
}