using Shopway.Domain.Users;
using Shopway.Domain.Errors;
using Microsoft.AspNetCore.Identity;
using Shopway.Domain.Common.Results;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LogUser;

internal sealed class LogUserCommandHandler
(
    IUserRepository userRepository, 
    IJwtProvider jwtProvider, 
    IValidator validator, 
    IPasswordHasher<User> passwordHasher
) 
    : ICommandHandler<LogUserCommand, LogUserResponse>
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IValidator _validator = validator;

    public async Task<IResult<LogUserResponse>> Handle(LogUserCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Email> emailResult = Email.Create(command.Email);
        ValidationResult<Password> passwordResult = Password.Create(command.Password);

        _validator
            .Validate(emailResult)
            .Validate(passwordResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<LogUserResponse>();
        }

        User? user = await _userRepository
            .GetByEmailAsync(emailResult.Value, cancellationToken);

        _validator
            .If(user is null, thenError: Error.InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<LogUserResponse>();
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, passwordResult.Value.Value);

        _validator
            .If(result is PasswordVerificationResult.Failed, thenError: Error.InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<LogUserResponse>();
        }

        string token = _jwtProvider.GenerateJwt(user!);

        return new LogUserResponse(token)
            .ToResult();
    }
}
