using System.Text;
using System.Security.Claims;
using Shopway.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using Shopway.Infrastructure.Policies;
using Shopway.Application.Abstractions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Shopway.Infrastructure.Providers;

internal sealed class JwtProvider(IOptions<AuthenticationOptions> options, IDateTimeProvider dateTimeProvider) : IJwtProvider
{
    private readonly AuthenticationOptions _options = options.Value;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public string GenerateJwt(User user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username.Value),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(ClaimPolicies.CustomerId, user switch
            {
                { CustomerId: not null } => $"{user.CustomerId}",
                _ => string.Empty
            })
        };

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var expires = _dateTimeProvider.UtcNow.AddDays(_options.DaysToExpire);

        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires.DateTime,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
