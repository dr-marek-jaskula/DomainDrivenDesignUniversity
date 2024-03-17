using Shopway.Application.Features.Users.Commands.LogUser;
using Shopway.Domain.Users;

namespace Shopway.Application.Abstractions;

public interface IJwtProvider
{
    AccessTokenResult GenerateJwt(User user);
}