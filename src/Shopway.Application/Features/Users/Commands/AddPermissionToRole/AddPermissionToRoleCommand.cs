using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.AddPermissionToRole;

public sealed record AddPermissionToRoleCommand
(
    string Role,
    string Permission
)
    : ICommand;
