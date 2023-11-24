using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using System.Text.RegularExpressions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Common.BaseTypes;
using static Shopway.Domain.Errors.Domain.DomainErrors;
using static System.Text.RegularExpressions.RegexOptions;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Users.ValueObjects;

public sealed class Address : ValueObject
{
    public const int MaxFlatNumber = 1000;
    public const int MaxBuildingNumber = 1000;
    public const int MinFlatNumber = 1;
    public const int MinBuildingNumber = 1;
    public static readonly string[] AvailableCountries = ["Poland", "Germany", "England", "Russia"];
    private static readonly Regex _zipCodeRegex = new(@"^\d{5}?$", Compiled | CultureInvariant | Singleline, TimeSpan.FromMilliseconds(100));

    public string City { get; }
    public string Country { get; }
    public string ZipCode { get; }
    public string Street { get; }
    public int Building { get; }
    public int? Flat { get; }

    private Address(string city, string country, string zipCode, string street, int building, int? flat)
    {
        City = city;
        Country = country;
        ZipCode = zipCode;
        Street = street;
        Building = building;
        Flat = flat;
    }

    //Empty constructor in this case is required by EF Core, because has a complex type as a parameter in the default constructor.
    private Address()
    {
    }

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

    /// <returns>Street, City, Country, ZipCode, Building and Flat if not null</returns>
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return ZipCode;
        yield return Building;

        if (Flat is not null)
        {
            yield return Flat;
        }
    }

    private static IList<Error> ValidateCountry(IList<Error> errors, string country)
    {
        return errors
            .If(country.IsNullOrEmptyOrWhiteSpace(), AddressError.EmptyCountry)
            .If(AvailableCountries.NotContains(country), AddressError.UnsupportedCountry);
    }

    private static IList<Error> ValidateCity(IList<Error> errors, string city)
    {
        return errors
            .If(city.IsNullOrEmptyOrWhiteSpace(), AddressError.EmptyCity)
            .If(city.ContainsIllegalCharacter() || city.ContainsDigit(), AddressError.ContainsIllegalCharacterOrDigit);
    }

    private static IList<Error> ValidateZipCode(IList<Error> errors, string zipCode)
    {
        return errors
            .If(_zipCodeRegex.NotMatch(zipCode), AddressError.ZipCodeDoesNotMatch);
    }

    private static IList<Error> ValidateStreet(IList<Error> errors, string street)
    {
        return errors
            .If(street.IsNullOrEmptyOrWhiteSpace(), AddressError.EmptyCity)
            .If(street.ContainsIllegalCharacter(), AddressError.ContainsIllegalCharacter);
    }

    private static IList<Error> ValidateBuilding(IList<Error> errors, int building)
    {
        return errors
            .If(building < MinBuildingNumber || building > MaxBuildingNumber, AddressError.WrongBuildingNumber);
    }

    private static IList<Error> ValidateFlat(IList<Error> errors, int? flat)
    {
        return errors
            .If(flat is not null && (flat < MinFlatNumber || flat > MaxFlatNumber), AddressError.WrongFlatNumber);
    }
}

