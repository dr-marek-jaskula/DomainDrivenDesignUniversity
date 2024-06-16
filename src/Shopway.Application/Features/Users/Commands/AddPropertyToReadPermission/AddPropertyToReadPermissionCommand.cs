using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.AddPropertyToReadPermission;

public sealed record AddPropertyToReadPermissionCommand
(
    string Permission,
    string Property
)
    : ICommand;
