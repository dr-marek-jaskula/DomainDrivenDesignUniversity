using Shopway.Domain.Entities;
using Shopway.Application.Features.Users.Queries;
using Shopway.Application.Features.Users.Commands.RegisterUser;

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
}