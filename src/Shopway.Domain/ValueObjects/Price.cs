﻿using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class Price : ValueObject
{
    public const decimal MaxPrice = 100000;
    public const decimal MinPrice = 0;

    public decimal Value { get; }

    private Price(decimal value)
    {
        Value = value;
    }

    public static ValidationResult<Price> Create(decimal price)
    {
        var errors = Validate(price);
        return errors.CreateValidationResult(() => new Price(decimal.Round(price, 2)));
    }

    public static IList<Error> Validate(decimal price)
    {
        return EmptyList<Error>()
            .If(price < MinPrice, PriceError.TooLow)
            .If(price > MaxPrice, PriceError.TooHigh);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

