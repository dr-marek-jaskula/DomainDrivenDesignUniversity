using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.RemovePermissionFromRole;

public sealed record RemovePermissionFromRoleCommand
(
    string Role,
    string Permission
)
    : ICommand;
