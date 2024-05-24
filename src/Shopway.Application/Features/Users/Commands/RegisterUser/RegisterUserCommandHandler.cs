using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(IUserRepository userRepository, IValidator validator, IPasswordHasher<User> passwordHasher)
    : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IValidator _validator = validator;

    public async Task<IResult<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email).Value;

        bool emailIsTaken = await _userRepository
            .IsEmailTakenAsync(email, cancellationToken);

        _validator
            .If(emailIsTaken, thenError: Email.AlreadyTaken);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RegisterUserResponse>();
        }

        return RegisterUser(email, Username.Create(command.Username).Value, Password.Create(command.Password).Value);
    }

    private IResult<RegisterUserResponse> RegisterUser(Email email, Username username, Password password)
    {
        var user = User.Create(UserId.New(), username, email);

        ValidationResult<PasswordHash> passwordHashResult = PasswordHash.Create(_passwordHasher.HashPassword(user, password.Value));

        _validator
            .Validate(passwordHashResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RegisterUserResponse>();
        }

        user.SetHashedPassword(passwordHashResult.Value);

        _userRepository.Add(user);

        return user
            .ToCreateResponse()
            .ToResult();
    }
}
