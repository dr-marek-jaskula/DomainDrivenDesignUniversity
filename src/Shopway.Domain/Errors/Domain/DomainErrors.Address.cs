using Shopway.Domain.Utilities;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class AddressError
    {
        public static readonly Error EmptyCountry = Error.New(
            $"{nameof(Address)}.{nameof(EmptyCountry)}",
            "Country is empty.");

        public static readonly Error UnsupportedCountry = Error.New(
            $"{nameof(Address)}.{nameof(UnsupportedCountry)}",
            $"Country name must be: {Address.AvailableCountries.Join(',')}.");

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
            $"Building number must be positive and at most {Address.MaxBuildingNumber} and at least {Address.MinBuildingNumber}.");

        public static readonly Error WrongFlatNumber = Error.New(
            $"{nameof(Address)}.{nameof(WrongFlatNumber)}",
            $"Flat number must be positive and at most {Address.MaxFlatNumber} and at least {Address.MinFlatNumber}.");

        public static readonly Error Invalid = Error.New(
            $"{nameof(Address)}.{nameof(Invalid)}",
            $"{nameof(Address)} must consist of nine digits and cannot start from zero.");
    }
}