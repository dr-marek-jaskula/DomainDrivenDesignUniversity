﻿using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using static Shopway.Domain.Errors.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Username : ValueObject
{
    public const int MaxLength = 30;

    private Username(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ValidationResult<Username> Create(string username)
    {
        var errors = Validate(username);

        if (errors.Any())
        {
            return ValidationResult<Username>.WithErrors(errors.ToArray());
        }

        return ValidationResult<Username>.WithoutErrors(new Username(username));
    }

    private static List<Error> Validate(string username)
    {
        var errors = Empty<Error>();

        if (string.IsNullOrWhiteSpace(username))
        {
            errors.Add(UsernameError.Empty);
        }

        if (username.Length > MaxLength)
        {
            errors.Add(UsernameError.TooLong);
        }

        if (username.ContainsIllegalCharacter())
        {
            errors.Add(UsernameError.ContainsIllegalCharacter);
        }

        return errors;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
