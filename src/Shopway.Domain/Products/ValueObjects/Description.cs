﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed record class Description : ValueObject<string>
{
    public const int MaxLength = 600;

    public static readonly Error Empty = Error.New(
        $"{nameof(Description)}.{nameof(Empty)}",
        $"{nameof(Description)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Description)}.{nameof(TooLong)}",
        $"{nameof(Description)} needs to be at most {MaxLength} characters long.");

    private Description(string description) : base(description)
    {
    }

    public static ValidationResult<Description> Create(string description)
    {
        var errors = Validate(description);
        return errors.CreateValidationResult(() => new Description(description));
    }

    public static IList<Error> Validate(string description)
    {
        return EmptyList<Error>()
            .If(description.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(description.Length > MaxLength, TooLong);
    }
}
