using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.RemovePropertyFromReadPermission;

public sealed record RemovePropertyFromReadPermissionCommand
(
    string Permission,
    string Property
)
    : ICommand;
