using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Text.RegularExpressions;
using static Shopway.Domain.Common.Utilities.ListUtilities;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Users.ValueObjects;

public sealed record class Address : IValueObject
{
    public const int MaxFlatNumber = 1000;
    public const int MaxBuildingNumber = 1000;
    public const int MinFlatNumber = 1;
    public const int MinBuildingNumber = 1;
    public static readonly string[] AvailableCountries = ["Poland", "Germany", "England", "Russia"];
    private static readonly Regex _zipCodeRegex = new(@"^\d{5}?$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public static readonly Error EmptyCountry = Error.New(
        $"{nameof(Address)}.{nameof(EmptyCountry)}",
        "Country is empty.");

    public static readonly Error UnsupportedCountry = Error.New(
        $"{nameof(Address)}.{nameof(UnsupportedCountry)}",
        $"Country name must be: {AvailableCountries.Join(',')}.");

    public static readonly Error EmptyCity = Error.New(
        $"{nameof(Address)}.{nameof(EmptyCity)}",
        "City is empty.");

    public static readonly Error ContainsIllegalCharacterOrDigit = Error.New(
        $"{nameof(Address)}.{nameof(ContainsIllegalCharacterOrDigit)}",
        "City contains illegal character or digit.");

    public static readonly Error ZipCodeDoesNotMatch = Error.New(
        $"{nameof(Address)}.{nameof(ZipCodeDoesNotMatch)}",
        "Zip code must consist of 5 digits.");

    public static readonly Error ContainsIllegalCharacter = Error.New(
        $"{nameof(Address)}.{nameof(ContainsIllegalCharacter)}",
        "Street contains illegal character.");

    public static readonly Error WrongBuildingNumber = Error.New(
        $"{nameof(Address)}.{nameof(WrongBuildingNumber)}",
        $"Building number must be positive and at most {MaxBuildingNumber} and at least {MinBuildingNumber}.");

    public static readonly Error WrongFlatNumber = Error.New(
        $"{nameof(Address)}.{nameof(WrongFlatNumber)}",
        $"Flat number must be positive and at most {MaxFlatNumber} and at least {MinFlatNumber}.");

    public static readonly Error Invalid = Error.New(
        $"{nameof(Address)}.{nameof(Invalid)}",
        $"{nameof(Address)} must consist of nine digits and cannot start from zero.");

    private Address(string city, string country, string zipCode, string street, int building, int? flat)
    {
        City = city;
        Country = country;
        ZipCode = zipCode;
        Street = street;
        Building = building;
        Flat = flat;
    }

    private Address()
    {
    }

    public string City { get; }
    public string Country { get; }
    public string ZipCode { get; }
    public string Street { get; }
    public int Building { get; }
    public int? Flat { get; }

    public static ValidationResult<Address> Create(string city, string country, string zipCode, string street, int building, int? flat)
    {
        var errors = Validate(city, country, zipCode, street, building, flat);
        return errors.CreateValidationResult(() => new Address(city, country, zipCode, street, building, flat));
    }

    public static IList<Error> Validate(string city, string country, string zipCode, string street, int building, int? flat)
    {
        return EmptyList<Error>()
            .UseValidation(ValidateCountry, country)
            .UseValidation(ValidateCity, city)
            .UseValidation(ValidateZipCode, zipCode)
            .UseValidation(ValidateStreet, street)
            .UseValidation(ValidateBuilding, building)
            .UseValidation(ValidateFlat, flat);
    }

    private static IList<Error> ValidateCountry(IList<Error> errors, string country)
    {
        return errors
            .If(country.IsNullOrEmptyOrWhiteSpace(), EmptyCountry)
            .If(AvailableCountries.NotContains(country), UnsupportedCountry);
    }

    private static IList<Error> ValidateCity(IList<Error> errors, string city)
    {
        return errors
            .If(city.IsNullOrEmptyOrWhiteSpace(), EmptyCity)
            .If(city.ContainsIllegalCharacter() || city.ContainsDigit(), ContainsIllegalCharacterOrDigit);
    }

    private static IList<Error> ValidateZipCode(IList<Error> errors, string zipCode)
    {
        return errors
            .If(_zipCodeRegex.NotMatch(zipCode), ZipCodeDoesNotMatch);
    }

    private static IList<Error> ValidateStreet(IList<Error> errors, string street)
    {
        return errors
            .If(street.IsNullOrEmptyOrWhiteSpace(), EmptyCity)
            .If(street.ContainsIllegalCharacter(), ContainsIllegalCharacter);
    }

    private static IList<Error> ValidateBuilding(IList<Error> errors, int building)
    {
        return errors
            .If(building < MinBuildingNumber || building > MaxBuildingNumber, WrongBuildingNumber);
    }

    private static IList<Error> ValidateFlat(IList<Error> errors, int? flat)
    {
        return errors
            .If(flat is not null && (flat < MinFlatNumber || flat > MaxFlatNumber), WrongFlatNumber);
    }
}

