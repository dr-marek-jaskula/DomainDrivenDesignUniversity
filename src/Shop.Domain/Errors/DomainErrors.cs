namespace Shopway.Domain.Errors;

//The static class that gathers all domain errors in one place
public static class DomainErrors
{
    public static class RoleName
    {
        public static readonly Error Empty = new(
            "RoleName.Empty",
            "Role name is empty");

        public static readonly Error Invalid = new(
            "RoleName.Invalid",
            $"RoleName name must be: {string.Join(',', ValueObjects.RoleName.AllowedRoles)}");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "FirstName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    public static class Member
    {
        public static readonly Error EmailAlreadyInUse = new(
            "Member.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Member.NotFound",
            $"The member with the identifier {id} was not found.");
    }

    public static class Gathering
    {
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

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }
}
