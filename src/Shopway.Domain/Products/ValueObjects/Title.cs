using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Title : ValueObject
{
    public const int MaxLength = 45;

    public static readonly Error Empty = Error.New(
        $"{nameof(Title)}.{nameof(Empty)}",
        $"{nameof(Title)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Title)}.{nameof(TooLong)}",
        $"{nameof(Title)} needs to be at most {MaxLength} characters long.");

    private Title(string value)
    {
        Value = value;
    }

    //For EF Core
    private Title()
    {
    }

    public new string Value { get; }

    public static ValidationResult<Title> Create(string title)
    {
        var errors = Validate(title);
        return errors.CreateValidationResult(() => new Title(title));
    }

    public static IList<Error> Validate(string title)
    {
        return EmptyList<Error>()
            .If(title.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(title.Length > MaxLength, TooLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
