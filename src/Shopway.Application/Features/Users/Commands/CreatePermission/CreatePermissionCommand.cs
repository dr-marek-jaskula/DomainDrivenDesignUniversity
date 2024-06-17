using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.CreatePermission;

public sealed record CreatePermissionCommand
(
    string PermissionName
)
    : ICommand;
