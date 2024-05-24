using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Users.Commands.Revoke;

internal sealed class RevokeRefreshTokenCommandHandler
(
    IUserRepository userRepository,
    IValidator validator
)
    : ICommandHandler<RevokeRefreshTokenCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userRepository
            .GetByIdAsync(command.UserId, cancellationToken);

        _validator
            .If(user is null, thenError: Error.InvalidReference(command.UserId.Value, nameof(User)));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        user!.Revoke();

        return Result.Success();
    }
}
