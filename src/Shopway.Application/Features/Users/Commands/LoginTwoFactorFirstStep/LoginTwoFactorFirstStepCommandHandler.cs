using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using static Shopway.Domain.Users.Errors.DomainErrors.PasswordOrEmailError;
using static Shopway.Domain.Common.Utilities.RandomUtilities;
using static Shopway.Domain.Users.ValueObjects.TwoFactorTokenHash;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstStep;

internal sealed class LoginTwoFactorFirstStepCommandHandler
(
    IUserRepository userRepository,
    IValidator validator,
    IPasswordHasher<User> passwordHasher
)
    : ICommandHandler<LoginTwoFactorFirstStepCommand>
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(LoginTwoFactorFirstStepCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email).Value;

        User? user = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        _validator
            .If(user is null, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var password = Password.Create(command.Password).Value;

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, password.Value);

        _validator
            .If(result is PasswordVerificationResult.Failed, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var twoFactorTokenAsString = $"{GenerateString(NotHashedTokenFirstPartLength)}{NotHashedTokenSeparator}{GenerateString(NotHashedTokenSecondPartLength)}";
        var twoFactorTokenHash = _passwordHasher.HashPassword(user, twoFactorTokenAsString);
        var twoFactorToken = TwoFactorTokenHash.Create(twoFactorTokenHash);

        _validator
            .Validate(twoFactorToken);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        user.SetTwoFactorToken(twoFactorToken.Value, twoFactorTokenAsString);

        return Result.Success();
    }
}
