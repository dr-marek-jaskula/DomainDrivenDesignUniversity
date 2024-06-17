using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Features.Users.Commands.CreatePermission;

internal sealed class CreatePermissionCommandHandler(IAuthorizationRepository authorizationRepository, IValidator validator)
    : ICommandHandler<CreatePermissionCommand>
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly IValidator _validator = validator;

    public async Task<IResult> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}
