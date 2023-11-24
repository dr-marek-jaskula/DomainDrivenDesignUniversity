using Shopway.Domain.Users;

namespace Shopway.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateJwt(User user);
}