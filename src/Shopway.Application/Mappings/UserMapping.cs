using Shopway.Application.Features.Users.Commands.RegisterUser;
using Shopway.Application.Features.Users.Queries.GetRolePermissions;
using Shopway.Application.Features.Users.Queries.GetUserByUsername;
using Shopway.Application.Features.Users.Queries.GetUserRoles;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Enumerations;

namespace Shopway.Application.Mappings;

public static class UserMapping
{
    public static RegisterUserResponse ToCreateResponse(this User userToCreate)
    {
        return new RegisterUserResponse(userToCreate.Id.Value);
    }

    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        (
            user.Id.Value,
            user.Username.Value,
            user.Email.Value,
            user.CustomerId?.Value
        );
    }

    public static RolesResponse ToResponse(this IReadOnlyCollection<Role> roles)
    {
        return new RolesResponse(roles.Select(role => role.Name).ToList());
    }

    public static RolePermissionsResponse ToResponse(this Role role)
    {
        return new RolePermissionsResponse(role.Permissions.Select(x => x.Name).ToList());
    }
}
