using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Application.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.Results;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Application.CQRS.Users.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator _validator;

    public RegisterUserCommandHandler(IUserRepository userRepository, IValidator validator, IPasswordHasher<User> passwordHasher)
    {
        _validator = validator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IResult<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        ValidationResult<Email> emailResult = Email.Create(request.Email);
        ValidationResult<Username> usernameResult = Username.Create(request.Username);
        ValidationResult<Password> passwordResult = Password.Create(request.Password);

        bool emailIsUnique = await _userRepository
            .IsEmailUniqueAsync(emailResult.Value, cancellationToken);

        _validator
            .Validate(emailResult)
            .Validate(usernameResult)
            .Validate(passwordResult)
            .If(emailIsUnique is false, thenError: EmailError.EmailAlreadyTaken);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<RegisterUserResponse>();
        }

        var result = AddUser(emailResult.Value, usernameResult.Value, passwordResult.Value);

        return result;
    }

    private IResult<RegisterUserResponse> AddUser(Email email, Username username, Password password)
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