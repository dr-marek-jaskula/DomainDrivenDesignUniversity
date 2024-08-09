using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Shopway.Application.Features.Orders.Commands.SoftDeleteOrderHeader;
using Shopway.Domain.Orders;
using Shopway.Presentation.Abstractions;
using Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;
using Shopway.Presentation.Utilities;
using System.Security.Claims;

namespace Shopway.Presentation.MinimalEndpoints.Orders;

public sealed class SoftDeleteOrderHeaderMinimalEndpoint : IEndpoint<OrderHeadersGroup>
{
    private const string _name = nameof(SoftDeleteOrderHeaderMinimalEndpoint);
    private const string _summary = "Mark order header as deleted";
    private const string _description = "This documentation is for tutorial purpose - to demonstrate how to provide the OpenApi documentation for MinimalApi Endpoints";

    public static void RegisterEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", SoftDeleteOrderHeader)
            .WithName(_name)
            .WithDescription(_description)
            .WithSummaryAuth(_summary)
            .WithVersion(VersionGroup.OrderHeaders, 1, 0);
    }

    private static async Task<Results<Ok, ProblemHttpResult, ForbidHttpResult>> SoftDeleteOrderHeader
    (
        OrderHeaderId id,
        ISender sender,
        IAuthorizationService authorizationService,
        ClaimsPrincipal user,
        CancellationToken cancellationToken
    )
    {
        var authorizationResult = await authorizationService.AuthorizeAsync(user, id, OrderHeaderCreatedByUserRequirement.PolicyName);

        if (authorizationResult.Succeeded is false)
        {
            return authorizationResult.ToForbidResult();
        }

        var command = new SoftDeleteOrderHeaderCommand(id);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure
            ? result.ToProblemHttpResult()
            : result.ToOkResult();
    }
}
