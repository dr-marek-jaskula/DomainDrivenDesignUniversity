﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Users.Errors.DomainErrors;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public new string Value { get; }

    public static ValidationResult<FirstName> Create(string firstName)
    {
        var errors = Validate(firstName);
        return errors.CreateValidationResult(() => new FirstName(firstName));
    }

    public static IList<Error> Validate(string firstName)
    {
        return EmptyList<Error>()
            .If(firstName.IsNullOrEmptyOrWhiteSpace(), FirstNameError.Empty)
            .If(firstName.Length > MaxLength, FirstNameError.TooLong)
            .If(firstName.ContainsIllegalCharacter(), FirstNameError.ContainsIllegalCharacter)
            .If(firstName.ContainsDigit(), FirstNameError.ContainsDigit);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
