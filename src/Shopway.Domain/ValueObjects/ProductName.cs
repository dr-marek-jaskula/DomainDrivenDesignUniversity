﻿using Shopway.Domain.Utilities;
using Shopway.Domain.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Domain.ValueObjects;

public sealed class ProductName : ValueObject
{
    public const int MaxLength = 50;

    public new string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static ValidationResult<ProductName> Create(string productName)
    {
        var errors = Validate(productName);
        return errors.CreateValidationResult(() => new ProductName(productName));
    }

    public static IList<Error> Validate(string productName)
    {
        return EmptyList<Error>()
            .If(productName.IsNullOrEmptyOrWhiteSpace(), ProductNameError.Empty)
            .If(productName.Length > MaxLength, ProductNameError.TooLong)
            .If(productName.ContainsIllegalCharacter(), ProductNameError.ContainsIllegalCharacter);
    }
}