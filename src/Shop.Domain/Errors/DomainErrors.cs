namespace Shopway.Domain.Errors;

//The static class that gathers all domain errors in one place
public static class DomainErrors
{
    public static class RoleNameError
    {
        public static readonly Error Empty = new(
            "RoleName.Empty",
            "Role name is empty");

        public static readonly Error Invalid = new(
            "RoleName.Invalid",
            $"RoleName name must be: {string.Join(',', ValueObjects.RoleName.AllowedRoles)}");
    }

    public static class FirstNameError
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "FirstName.TooLong",
            "FirstName name is too long");
    }

    public static class LastNameError
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    public static class EmailError
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error Invalid = new(
            "Email.Invalid",
            $"Email must start from a letter, contain '@' and after that '.'");
    }

    public static class PhoneNumberError
    {
        public static readonly Error Empty = new(
            "PhoneNumber.Empty",
            "PhoneNumber is empty");

        public static readonly Error Invalid = new(
            "PhoneNumber.Invalid",
            $"PhoneNumber must consist of nine digits and cannot start from zero");
    }

    public static class AddressError
    {
        public static readonly Error EmptyCountry = new(
            "Address.EmptyCountry",
            "Country is empty");

        public static readonly Error UnsupportedCountry = new(
            "Address.UnsupportedCountry",
            "This country is unsupported");

        public static readonly Error EmptyCity = new(
            "Address.EmptyCity",
            "City is empty");

        public static readonly Error ContainsIllegalCharacterOrDigit = new(
            "Address.ContainsIllegalCharacterOrDigit",
            "City contains illegal character or digit");

        public static readonly Error ZipCodeDoesNotMatch = new(
            "Address.ContainsIllegalCharacterOrDigit",
            "Zip code must consist of 5 digits");

        public static readonly Error ContainsIllegalCharacter = new(
            "Address.ContainsIllegalCharacter",
            "Street contains illegal character");

        public static readonly Error WrongBuildingNumber = new(
            "Address.WrongBuildingNumber",
            "Building number must be positive and less than 1000");

        public static readonly Error WrongFlatNumber = new(
            "Address.WrongFlatNumber",
            "Flat number must be positive and less than 1000");

        public static readonly Error Invalid = new(
            "Address.Invalid",
            $"Address must consist of nine digits and cannot start from zero");
    }

    public static class GatheringError
    {
        public static readonly Error EmailAlreadyInUse = new(
            "Member.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Gathering.NotFound",
            $"The gathering with the identifier {id} was not found.");

        public static readonly Error InvitingCreator = new(
            "Gathering.InvitingCreator",
            "Can't send invitation to the gathering creator");

        public static readonly Error AlreadyPassed = new(
            "Gathering.AlreadyPassed",
            "Can't send invitation for gathering in the past");

        public static readonly Error Expired = new(
            "Gathering.Expired",
            "Can't accept invitation for expired gathering");
    }
}
