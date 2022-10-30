﻿using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class Revision : ValueObject
{
    public const int MaxLength = 10;

    public string Value { get; }

    private Revision(string value)
    {
        Value = value;
    }

    //For EF Core
    private Revision()
    {
    }

    public static Result<Revision> Create(string revision)
    {
        if (string.IsNullOrWhiteSpace(revision))
        {
            return Result.Failure<Revision>(DomainErrors.RevisionError.Empty);
        }

        if (revision.Length > MaxLength)
        {
            return Result.Failure<Revision>(DomainErrors.RevisionError.TooLong);
        }

        return new Revision(revision);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

