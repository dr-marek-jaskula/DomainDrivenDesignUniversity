using Shopway.Application.Abstractions.CQRS;
using System.Security.Claims;

namespace Shopway.Application.Features.Users.Commands.LoginByGoogle;

public sealed record LoginByGoogleCommand(ClaimsPrincipal ClaimsPrincipal) : ICommand<AccessTokenResponse>;
