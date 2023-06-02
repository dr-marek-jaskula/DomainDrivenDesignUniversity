using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Application.Utilities;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Users.Commands.LogUser;

internal sealed class LogUserCommandHandler : ICommandHandler<LogUserCommand, LogUserResponse>
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IValidator _validator;

    public LogUserCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IValidator validator, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }

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
            .If(user is null, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<LogUserResponse>();
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, passwordResult.Value.Value);

        _validator
            .If(result is PasswordVerificationResult.Failed, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<LogUserResponse>();
        }

        string token = _jwtProvider.GenerateJwt(user!);

        return new LogUserResponse(token)
            .ToResult();
    }
}
