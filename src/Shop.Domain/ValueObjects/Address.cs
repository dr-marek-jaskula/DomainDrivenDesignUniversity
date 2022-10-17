using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Domain.ValueObjects;

public sealed class Address : ValueObject
{
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

    //TODO make Address Validation
    public static Result<Address> Create(string city, string country, string zipCode, string street, int building, int? flat)
    {
        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
        {
            return Result.Failure<Address>(DomainErrors.AddressError.EmptyCity);
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
}