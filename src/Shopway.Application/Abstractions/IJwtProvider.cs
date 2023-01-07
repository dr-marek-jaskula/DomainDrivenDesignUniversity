using Shopway.Domain.Entities;

namespace Shopway.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateJwt(User user);
}