using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using static Shopway.Domain.Errors.DomainErrors;

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

    public static Result<Title> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure<Title>(TitleError.Empty);
        }

        if (title.Length > MaxLength)
        {
            return Result.Failure<Title>(TitleError.TooLong);
        }

        return new Title(title);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
