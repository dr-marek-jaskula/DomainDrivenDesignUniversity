using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Errors;

//The static class that gathers all domain errors in one place
public static class DomainErrors
{
    public static class RoleNameError
    {
        public static readonly Error Invalid = new(
            "RoleName.Invalid",
            $"RoleName name must be: {string.Join(',', ValueObjects.RoleName.AllowedRoles)}");
    }

    public static class UsernameError
    {
        public static readonly Error Empty = new(
            "Username.Empty",
            "Username name is empty");

        public static readonly Error TooLong = new(
            "Username.TooLong",
            $"Username name must be at most {Username.MaxLength} characters");

        public static readonly Error ContainsIllegalCharacter = new(
            "Username.ContainsIllegalCharacter",
            "Username contains illegal character");
    }

    public static class PasswordError
    {
        public static readonly Error Empty = new(
            "Password.Empty",
            "Password is empty");

        public static readonly Error TooShort = new(
            "Password.TooShort",
            $"Password needs to be at least {Password.MinLength} characters long");
        
        public static readonly Error TooLong = new(
            "Password.TooShort",
            $"Password needs to be at most {Password.MaxLength} characters long");

        public static readonly Error Invalid = new(
            "Password.Invalid",
            "Password needs to contain at least one digit, one small letter and one capital letter");
    }

    public static class PasswordHashError
    {
        public static readonly Error Empty = new(
            "PasswordHash.Empty",
            "PasswordHash is empty");

        public static readonly Error BytesLong = new(
            "PasswordHash.BytesLong",
            $"PasswordHash needs to be {PasswordHash.BytesLong} bytes long");
    }

    public static class FirstNameError
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "FirstName is empty");

        public static readonly Error TooLong = new(
            "FirstName.TooLong",
            $"FirstName must be at most {FirstName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            "FirstName.ContainsIllegalCharacter",
            "FirstName contains illegal character");

        public static readonly Error ContainsDigit = new(
            "FirstName.ContainsDigit",
            "FirstName contains digit");
    }

    public static class LastNameError
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "LastName is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            $"LastName must be at most {LastName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            "LastName.ContainsIllegalCharacter",
            "LastName contains illegal character");

        public static readonly Error ContainsDigit = new(
            "LastName.ContainsDigit",
            "LastName contains digit");
    }

    public static class ProductNameError
    {
        public static readonly Error Empty = new(
            "ProductName.Empty",
            "ProductName is empty");

        public static readonly Error TooLong = new(
            "ProductName.TooLong",
            $"ProductName must be at most {ProductName.MaxLength} characters long");

        public static readonly Error ContainsIllegalCharacter = new(
            "ProductName.ContainsIllegalCharacter",
            "ProductName contains illegal character");
    }

    public static class RevisionError
    {
        public static readonly Error Empty = new(
            "Revision.Empty",
            "Revision is empty");

        public static readonly Error TooLong = new(
            "Revision.TooLong",
            $"Revision must be at most {Revision.MaxLength} characters long");
    }

    public static class EmailError
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error TooLong = new(
            "Email.TooLong",
            $"Email must be at most {Email.MaxLength} characters long");

        public static readonly Error Invalid = new(
            "Email.Invalid",
            "Email must start from a letter, contain '@' and after that '.'");
    }

    public static class PhoneNumberError
    {
        public static readonly Error Empty = new(
            "PhoneNumber.Empty",
            "PhoneNumber is empty");

        public static readonly Error Invalid = new(
            "PhoneNumber.Invalid",
            "PhoneNumber must consist of 9 digits and cannot start from zero");
    }

    public static class AddressError
    {
        public static readonly Error EmptyCountry = new(
            "Address.EmptyCountry",
            "Country is empty");

        public static readonly Error UnsupportedCountry = new(
            "Address.UnsupportedCountry",
            $"Country name must be: {string.Join(',', ValueObjects.Address.AvailableCountries)}");

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
            $"Building number must be positive and at most {Address.MaxBuildingNumber} and at least {Address.MinBuildingNumber}");

        public static readonly Error WrongFlatNumber = new(
            "Address.WrongFlatNumber",
            $"Flat number must be positive and at most {Address.MaxFlatNumber} and at least {Address.MinFlatNumber}");

        public static readonly Error Invalid = new(
            "Address.Invalid",
            $"Address must consist of nine digits and cannot start from zero");
    }

    public static class DescriptionError
    {
        public static readonly Error Empty = new(
            "Description.Empty",
            "Description is empty");

        public static readonly Error TooLong = new(
            "Description.TooLong",
            $"Description needs to be at most {Description.MaxLength} characters long");
    }

    public static class TitleError
    {
        public static readonly Error Empty = new(
            "Title.Empty",
            "Title is empty");

        public static readonly Error TooLong = new(
            "Title.TooLong",
            $"Title needs to be at most {Title.MaxLength} characters long");
    }

    public static class PriorityError
    {
        public static readonly Error TooHigh = new(
            "Priority.TooHigh",
            $"Priority must be at least {Priority.HighestPriority}");

        public static readonly Error TooLow = new(
            "Priority.TooLow",
            $"Priority must be at most {Priority.LowestPriority}");
    }

    public static class StoryPointsError
    {
        public static readonly Error TooLow = new(
            "StoryPoints.TooLow",
            $"StoryPoints must be at least {StoryPoints.MinStoryPoints}");

        public static readonly Error TooHigh = new(
            "StoryPoints.TooHigh",
            $"StoryPoints must be at most {StoryPoints.MaxStoryPoints}");
    }

    public static class StarsError
    {
        public static readonly Error Invalid = new(
            "Stars.TooLow",
            $"Stars name must be: {string.Join(',', Stars.AdmissibleStars)}");
    }

    public static class AmountError
    {
        public static readonly Error TooLow = new(
            "Amount.TooLow",
            $"Amount must be at least {Amount.MinAmount}");

        public static readonly Error TooHigh = new(
            "Amount.TooHigh",
            $"Amount must be at most {Amount.MaxAmount}");
    }

    public static class UomCodeError
    {
        public static readonly Error Invalid = new(
            "UomCode.Invalid",
            $"UomCode name must be: {string.Join(',', UomCode.AllowedUomCodes)}");
    }

    public static class WorkItemError
    {
        public static readonly Error AlreadyAssigned = new(
            "WorkItem.AlreadyAssigned",
            "Can't assign the same WorkItem twice");
    }

    public static class PriceError
    {
        public static readonly Error TooLow = new(
            "Price.TooLow",
            $"Price must be at least {Price.MinPrice}");

        public static readonly Error TooHigh = new(
            "Price.TooHigh",
            $"Price must be at most {Price.MaxPrice}");
    }

    public static class DiscountError
    {
        public static readonly Error TooLow = new(
            "Discount.TooLow",
            $"Discount must be at least {Discount.MinDiscount}");

        public static readonly Error TooHigh = new(
            "Discount.TooHigh",
            $"Discount must be at most {Discount.MaxDiscount}");
    }
}
