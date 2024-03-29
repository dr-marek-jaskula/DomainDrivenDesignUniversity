﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Products.Errors.DomainErrors;

namespace Shopway.Domain.Products.ValueObjects;

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

    public new string Value { get; }

    public static ValidationResult<Title> Create(string title)
    {
        var errors = Validate(title);
        return errors.CreateValidationResult(() => new Title(title));
    }

    public static IList<Error> Validate(string title)
    {
        return EmptyList<Error>()
            .If(title.IsNullOrEmptyOrWhiteSpace(), TitleError.Empty)
            .If(title.Length > MaxLength, TitleError.TooLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
