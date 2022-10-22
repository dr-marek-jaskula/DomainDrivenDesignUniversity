using Shopway.Domain.Errors;
using Shopway.Domain.Extensions;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using System.Text.RegularExpressions;

namespace Shopway.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public const int MaxFlatNumber = 1000;
    public const int MaxBuildingNumber = 1000;
    public static readonly string[] AvailableCountries = new string[4] { "Poland", "Germany", "England", "Russia" };
    private static readonly Regex _zipCodeRegex = new(@"^\d{5}?$", RegexOptions.Compiled);

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

    //TODO ask Milan about my validation
    public static Result<Address> Create(string city, string country, string zipCode, string street, int building, int? flat)
    {

        if (ValidateCountry(country) is { IsValid: false } countryValidation)
        {
            return Result.Failure<Address>(countryValidation.Error);
        }

        if (ValidateCity(city) is { IsValid: false } cityValidation)
        {
            return Result.Failure<Address>(cityValidation.Error);
        }

        if (ValidateZipCode(zipCode) is { IsValid: false } zipCodeValidation)
        {
            return Result.Failure<Address>(zipCodeValidation.Error);
        }

        if (ValidateStreet(street) is { IsValid: false } streetValidation)
        {
            return Result.Failure<Address>(streetValidation.Error);
        }

        if (ValidateBuilding(building) is { IsValid: false } buildingValidation)
        {
            return Result.Failure<Address>(buildingValidation.Error);
        }

        if (ValidateFlat(flat) is { IsValid: false } flatValidation)
        {
            return Result.Failure<Address>(flatValidation.Error);
        }

        return new Address(city, country, zipCode, street, building, flat);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return ZipCode;
        yield return Building;

        if (Flat is not null)
            yield return Flat;
    }

    #region Validation Methods

    private static (bool IsValid, Error Error) ValidateCountry(string country)
    {
        return country switch
        {
            string when country.IsNullOrEmptyOrWhiteSpace() => (false, DomainErrors.AddressError.EmptyCountry),
            string when AvailableCountries.Contains(country) => (false, DomainErrors.AddressError.UnsupportedCountry),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateCity(string city)
    {
        return city switch
        {
            string when city.IsNullOrEmptyOrWhiteSpace() => (false, DomainErrors.AddressError.EmptyCity),
            string when city.ContainsIllegalCharacter() || city.ContainsDigit() => (false, DomainErrors.AddressError.ContainsIllegalCharacterOrDigit),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateZipCode(string zipCode)
    {
        var result = _zipCodeRegex.Match(zipCode);

        return result.Success switch
        {
            false => (false, DomainErrors.AddressError.ZipCodeDoesNotMatch),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateStreet(string street)
    {
        return street switch
        {
            string when street.IsNullOrEmptyOrWhiteSpace() => (false, DomainErrors.AddressError.EmptyCity),
            string when street.ContainsIllegalCharacter() => (false, DomainErrors.AddressError.ContainsIllegalCharacter),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateBuilding(int building)
    {
        return building switch
        {
            <= 0 or > MaxBuildingNumber => (false, DomainErrors.AddressError.WrongBuildingNumber),
            _ => (true, Error.None)
        };
    }

    private static (bool IsValid, Error Error) ValidateFlat(int? flat)
    {
        return flat switch
        {
            null => (true, Error.None),
            <= 0 or > MaxFlatNumber => (false, DomainErrors.AddressError.WrongFlatNumber),
            _ => (true, Error.None)
        };
    }

    #endregion Validation Methods
}

