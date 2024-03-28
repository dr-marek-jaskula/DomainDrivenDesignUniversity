using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;

internal sealed class ConfigureTwoFactorToptLoginCommandHandler
(
    IUserRepository userRepository,
    IValidator validator,
    IToptService toptService
)
    : ICommandHandler<ConfigureTwoFactorToptLoginCommand, TwoFactorToptResponse>
{
    private readonly IToptService _toptService = toptService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<TwoFactorToptResponse>> Handle(ConfigureTwoFactorToptLoginCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Username> usernameResult = Username.Create(command.Username);

        _validator
            .Validate(usernameResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<TwoFactorToptResponse>();
        }

        User? user = await _userRepository
            .GetByUsernameAsync(usernameResult.Value, cancellationToken);

        _validator
            .If(user is null, thenError: Error.NotFound<User>(usernameResult.Value.Value));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<TwoFactorToptResponse>();
        }

        (string Secret, string QrCode) = _toptService.CreateSecret(user!.Username.Value);

        var twoFactorToptSecret = TwoFactorToptSecret.Create(Secret);

        _validator
            .Validate(twoFactorToptSecret);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<TwoFactorToptResponse>();
        }

        user.SetTwoFactorToptSecret(twoFactorToptSecret.Value);

        return new TwoFactorToptResponse(QrCode)
            .ToResult();
    }
}
