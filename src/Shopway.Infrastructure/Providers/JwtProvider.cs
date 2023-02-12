using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Infrastructure.Authentication;
using Shopway.Infrastructure.Policies;

namespace Shopway.Infrastructure.Providers;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly AuthenticationOptions _options;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtProvider(IOptions<AuthenticationOptions> options, IDateTimeProvider dateTimeProvider)
    {
        _options = options.Value;
        _dateTimeProvider = dateTimeProvider;
    }

    public string GenerateJwt(User user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username.Value),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(ClaimPolicies.PersonId, user switch
            {
                { PersonId: not null } => $"{user.PersonId}",
                _ => ""
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
