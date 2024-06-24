using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shopway.Application.Features.Users.Commands;
using Shopway.Application.Features.Users.Commands.LoginByGoogle;
using Shopway.Domain.Common.Utilities;
using Shopway.Presentation.Utilities;

namespace Shopway.Presentation.Controllers;

partial class UsersController
{
    public const string GoogleRedirectToGooglePath = "/api/google/redirect";
    public const string GoogleAuthenticatePath = "/api/google/authenticate";

    [HttpGet(GoogleRedirectToGooglePath)]
    [AllowAnonymous]
    [ProducesResponseType<Ok<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<Results<ProblemHttpResult, Ok>> RedirectToGoogleAuthenticate()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = GoogleAuthenticatePath
        });

        if (HttpContext.Response.Headers.Location.IsNullOrEmptyString())
        {
            return TypedResults.Problem("Location is empty");
        }

        return TypedResults.Ok();
    }

    [HttpGet(GoogleAuthenticatePath)]
    [AllowAnonymous]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult, BadRequest<IEnumerable<string>>>> GoogleAuthenticate(CancellationToken cancellationToken)
    {
        var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (response.Principal is null)
        {
            return TypedResults.BadRequest<IEnumerable<string>>(["none"]);
        }

        var command = new LoginByGoogleCommand(response.Principal);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
