using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Title : ValueObject
{
    public const int MaxLength = 45;

    private Title(string value)
    {
        Value = value;
    }

    //For EF Core
    private Title()
    {
    }

    public string Value { get; }

    public static ValidationResult<Title> Create(string title)
    {
        var errors = Validate(title);

        if (errors.Any())
        {
            return ValidationResult<Title>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Title>.WithoutErrors(new Title(title));
    }

    private static List<Error> Validate(string title)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(TitleError.Empty);
        }

        if (title.Length > MaxLength)
        {
            errors.Add(TitleError.TooLong);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
