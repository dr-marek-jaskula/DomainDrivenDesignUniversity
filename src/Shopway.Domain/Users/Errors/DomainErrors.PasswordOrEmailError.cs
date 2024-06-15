using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Users.Errors;

public static partial class DomainErrors
{
    public static class PasswordOrEmailError
    {
        /// <summary>
        /// Create an Error describing that a password or an email are invalid
        /// </summary>
        public static readonly Error InvalidPasswordOrEmail = Error.New(
            $"{nameof(User)}.{nameof(InvalidPasswordOrEmail)}",
            $"Invalid {nameof(Password)} or {nameof(Email)}.");
    }
}
