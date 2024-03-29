﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Products.Errors.DomainErrors;

namespace Shopway.Domain.Products.ValueObjects;

public sealed class Revision : ValueObject
{
    public const int MaxLength = 10;

    public new string Value { get; }

    private Revision(string value)
    {
        Value = value;
    }

    //For EF Core
    private Revision()
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
            .If(revision.IsNullOrEmptyOrWhiteSpace(), RevisionError.Empty)
            .If(revision.Length > MaxLength, RevisionError.TooLong);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

