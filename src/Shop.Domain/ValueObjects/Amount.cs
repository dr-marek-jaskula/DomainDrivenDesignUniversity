﻿using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class Amount : ValueObject
{
    public const int MaxAmount = 1000;
    public const int MinAmount = 1;

    public int Value { get; }

    private Amount(int value)
    {
        Value = value;
    }

    public static Result<Amount> Create(int amount)
    {
        if (amount < MinAmount)
        {
            return Result.Failure<Amount>(DomainErrors.AmountError.TooLow);
        }

        if (amount > MaxAmount)
        {
            return Result.Failure<Amount>(DomainErrors.AmountError.TooHigh);
        }

        return new Amount(amount);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

