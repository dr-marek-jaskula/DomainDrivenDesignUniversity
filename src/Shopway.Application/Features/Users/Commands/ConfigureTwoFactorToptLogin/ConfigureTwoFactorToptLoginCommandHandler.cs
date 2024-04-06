using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;

internal sealed class ConfigureTwoFactorToptLoginCommandHandler
(
    IUserRepository userRepository,
    IValidator validator,
    IToptService toptService,
    IUserContextService userContextService
)
    : ICommandHandler<ConfigureTwoFactorToptLoginCommand, TwoFactorToptResponse>
{
    private readonly IToptService _toptService = toptService;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult<TwoFactorToptResponse>> Handle(ConfigureTwoFactorToptLoginCommand command, CancellationToken cancellationToken)
    {
        var username = _userContextService.Username ?? string.Empty;
        ValidationResult<Username> usernameResult = Username.Create(username);

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

        (string secret, string qrCode) = _toptService.CreateSecret(user!.Username.Value);

        var twoFactorToptSecret = TwoFactorToptSecret.Create(secret);

        _validator
            .Validate(twoFactorToptSecret);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<TwoFactorToptResponse>();
        }

        user.SetTwoFactorToptSecret(twoFactorToptSecret.Value);

        return new TwoFactorToptResponse(qrCode)
            .ToResult();
    }
}
