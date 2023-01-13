using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

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

    public static Result<UomCode> Create(string uomCode)
    {
        if (!AllowedUomCodes.Contains(uomCode))
        {
            return Result.Failure<UomCode>(UomCodeError.Invalid);
        }

        return new UomCode(uomCode);
    }
}

