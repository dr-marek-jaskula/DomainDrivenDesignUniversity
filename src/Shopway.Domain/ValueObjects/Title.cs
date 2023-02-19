using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
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
        return errors.CreateValidationResult(() => new Title(title));
    }

    public static List<Error> Validate(string title)
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
