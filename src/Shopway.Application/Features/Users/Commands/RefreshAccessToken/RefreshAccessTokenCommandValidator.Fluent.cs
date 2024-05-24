using FluentValidation;
using Shopway.Application.Abstractions;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;
using System.Security.Claims;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Application.Features.Users.Commands.RefreshAccessToken;

internal sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator(ISecurityTokenService securityTokenService)
    {
        RuleFor(x => new { x.RefreshToken, x.AccessToken })
            .MustSatisfyValueObjectValidation(x => RefreshToken.Validate(x.RefreshToken))
            .MustSatisfyValueObjectValidation(x => AccessToken.Validate(x.AccessToken))
            .DependentRules(() => RuleFor(x => x.AccessToken)
                .MustSatisfy(accessToken =>
                {
                    var emailClaimResult = securityTokenService.GetClaimFromToken(accessToken, nameof(Email));

                    var errors = EmptyList<Error>();

                    if (emailClaimResult.IsFailure)
                    {
                        errors.Add(emailClaimResult.Error);
                        return errors;
                    }

                    if (emailClaimResult.Value is null)
                    {
                        errors.Add(Error.New($"{nameof(Email)}.{nameof(Claim)}", $"Missing {nameof(Email)} {nameof(Claim)} for given access token"));
                        return errors;
                    }

                    var emailErrors = Email.Validate(emailClaimResult.Value.Value);

                    return errors
                        .If(emailErrors.NotNullOrEmpty(), [.. emailErrors]);
                }));
    }
}
