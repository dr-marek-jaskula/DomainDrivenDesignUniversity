﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Products.ValueObjects;

public sealed record class Revision : ValueObject<string>
{
    public const int MaxLength = 10;

    public static readonly Error Empty = Error.New(
        $"{nameof(Revision)}.{nameof(Empty)}",
        $"{nameof(Revision)} is empty.");

    public static readonly Error TooLong = Error.New(
        $"{nameof(Revision)}.{nameof(TooLong)}",
        $"{nameof(Revision)} must be at most {MaxLength} characters long.");

    private Revision(string revision) : base(revision)
    {
    }


    public static ValidationResult<Revision> Create(string revision)
    {
        var errors = Validate(revision);
        return errors.CreateValidationResult(() => new Revision(revision));
    }

    public static IList<Error> Validate(string revision)
    {
        return EmptyList<Error>()
            .If(revision.IsNullOrEmptyOrWhiteSpace(), Empty)
            .If(revision.Length > MaxLength, TooLong);
    }
}

